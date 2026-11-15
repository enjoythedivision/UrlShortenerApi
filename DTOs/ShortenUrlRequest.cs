using System.ComponentModel.DataAnnotations;

namespace UrlShortenerApi.DTOs
{
    public class ShortenUrlRequest
    {
        [Required]
        [Url]
        public string OriginalUrl { get; set; } = string.Empty;
    }
}
