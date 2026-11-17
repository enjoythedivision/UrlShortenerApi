using System.ComponentModel.DataAnnotations;

namespace UrlShortenerApi.DTOs
{
    public class ShortenRequest
    {
        [Required]
        [Url]
        [StringLength(2048)]
        public string OriginalUrl { get; set; } = string.Empty;
    }
}
