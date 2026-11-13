using Microsoft.EntityFrameworkCore;

namespace UrlShortenerApi.Models
{
    [Index(nameof(ShortCode), IsUnique = true)]
    public class ShortUrl
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty;
    }
}
