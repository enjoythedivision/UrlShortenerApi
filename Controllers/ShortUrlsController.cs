using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlShortenerApi.DTOs;

namespace UrlShortenerApi.Controllers
{
    [Route("shorten")]
    [ApiController]
    public class ShortUrlsController : ControllerBase
    {
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
        public IActionResult Shorten(ShortenUrlRequest url)
        {
            return Ok(url.OriginalUrl);
        }
    }
}
