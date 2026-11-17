namespace UrlShortenerApi.DTOs
{
    public class ShortenResponse
    {
        public string OriginalUrl { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;
    }
}
