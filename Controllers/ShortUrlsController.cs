using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlShortenerApi.DTOs;

namespace UrlShortenerApi.Controllers
{
    [Route("shorten")]
    [ApiController]
    public class ShortUrlsController : ControllerBase
    {
        [HttpPost]
        public IActionResult Shorten(ShortenUrlRequest url)
        {
            return Ok(url.OriginalUrl);
        }
    }
}
