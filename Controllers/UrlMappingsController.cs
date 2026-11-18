using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Data;
using UrlShortenerApi.DTOs;
using UrlShortenerApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client.NativeInterop;

namespace UrlShortenerApi.Controllers
{
    [Route("shorten")]
    [ApiController]
    public class UrlMappingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UrlMappingsController(AppDbContext context)
        {
            _context = context;
        }

        private static string GenerateCode()
        {
            const string characters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            string code = "";

            for (int i = 0; i < 6; i++)
            {
                int index = Random.Shared.Next(characters.Length);
                code += characters[index];
            }

            return code;
        }

        private ShortenResponse CreateResponse(UrlMapping mapping)
        {
            var response = new ShortenResponse
            {
                OriginalUrl = mapping.OriginalUrl,
                ShortUrl = Request.Scheme + "://" + Request.Host + "/" + mapping.Code
            };

            return response;
        }

        [HttpPost]
        public async Task<IActionResult> Shorten(ShortenRequest request)
        {
            var mapping = await _context.UrlMappings
                .FirstOrDefaultAsync(x => x.OriginalUrl == request.OriginalUrl);

            if (mapping is not null)
            {
                return Ok(CreateResponse(mapping));
            }

            string code = GenerateCode();

            while (await _context.UrlMappings.AnyAsync(x => x.Code == code))
            {
                code = GenerateCode();
            }

            mapping = new UrlMapping
            {
                OriginalUrl = request.OriginalUrl,
                Code = code
            };

            _context.UrlMappings.Add(mapping);

            const int maxAttempts = 3;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    await _context.SaveChangesAsync();
                    break;
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException is SqlException sqlError)
                    {
                        if (sqlError.Number == 2601)
                        {
                            Console.WriteLine("Couldn't create shortcode.");

                            if (attempt < maxAttempts)
                            {
                                mapping.Code = GenerateCode();
                                continue;
                            }
                        }
                    }

                    throw;
                }
            }

            var response = CreateResponse(mapping);

            return CreatedAtAction(
                nameof(Resolve),
                new { code = mapping.Code },
                response);
        }

        [HttpGet("/{code}")]
        public async Task<IActionResult> Resolve(string code)
        {
            var existingUrl = await _context.UrlMappings
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Code == code);

            if (existingUrl is null)
            {
                return NotFound();
            }

            return Redirect(existingUrl.OriginalUrl);
        }
    }
}
