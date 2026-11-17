using Microsoft.EntityFrameworkCore;

namespace UrlShortenerApi.Models
{
    [Index(nameof(Code), IsUnique = true)]
    public class UrlMapping
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
