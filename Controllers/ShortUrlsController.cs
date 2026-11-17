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
    public class ShortUrlsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ShortUrlsController(AppDbContext context)
        {
            _context = context;
        }

        private static string GenerateShortCode()
        {
            string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            char randomChar;
            string shortUrl = "";
        
            for (int i = 0; i < 6; i++)
            {
                int randomIndex = new Random().Next(0, 62);
                randomChar = characters[randomIndex];
                shortUrl += randomChar;
            }
            return shortUrl;
        }

        [HttpPost]
        public async Task<IActionResult> Shorten(ShortenUrlRequest request)
        {
            var existingUrl = await _context.ShortUrls.FirstOrDefaultAsync(x => x.OriginalUrl == request.OriginalUrl);
            
            if (existingUrl is not null)
            {
                return Ok(existingUrl);
            }

            string originalUrl = request.OriginalUrl;
            string shortCode = GenerateShortCode();

            while (await _context.ShortUrls.AnyAsync(x => x.ShortCode == shortCode))
            {
                shortCode = GenerateShortCode();
            }

            var shortUrl = new ShortUrl{
                OriginalUrl = originalUrl,
                ShortCode = shortCode
            };

            _context.ShortUrls.Add(shortUrl);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, shortUrl);
        }

        [HttpGet("/{shortCode}")]
        public async Task<IActionResult> Resolve(string shortCode)
        {
            var existingUrl = await _context.ShortUrls
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(x => x.ShortCode == shortCode);
            if (existingUrl is null)
            {
                return NotFound();
            }

            return Redirect(existingUrl.OriginalUrl);

        }
    }
}
