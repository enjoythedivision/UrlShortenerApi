using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Data;
using UrlShortenerApi.DTOs;
using UrlShortenerApi.Models;

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

        [HttpPost]
        public async Task<IActionResult> Shorten(ShortenRequest request)
        {
            var existingUrl = await _context.UrlMappings
                .FirstOrDefaultAsync(x => x.OriginalUrl == request.OriginalUrl);

            if (existingUrl is not null)
            {
                return Ok(existingUrl);
            }

            string originalUrl = request.OriginalUrl;
            string code = GenerateCode();

            while (await _context.UrlMappings.AnyAsync(x => x.Code == code))
            {
                code = GenerateCode();
            }

            var mapping = new UrlMapping
            {
                OriginalUrl = originalUrl,
                Code = code
            };

            _context.UrlMappings.Add(mapping);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, mapping);
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
