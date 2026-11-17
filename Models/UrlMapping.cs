using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace UrlShortenerApi.Models
{
    [Index(nameof(Code), IsUnique = true)]
    public class UrlMapping
    {
        public int Id { get; set; }
        [StringLength(2048)]
        public string OriginalUrl { get; set; } = string.Empty;
        [StringLength(6)]
        public string Code { get; set; } = string.Empty;
    }
}
