using System.ComponentModel.DataAnnotations;

namespace UrlShortenerApi.DTOs
{
    public class ShortenRequest
    {
        [Required]
        [Url]
        public string OriginalUrl { get; set; } = string.Empty;
    }
}
