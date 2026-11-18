**URL Shortener API**

A small learning project built with C#, ASP.NET Core, Entity Framework Core and SQL Server. It turns URLs into short links using six-character codes.

- `POST /shorten` accepts an `originalUrl` and returns the original and shortened URLs. Returns `201` for a new mapping or `200` for an existing one.
- `GET /{code}` redirects to the original URL, or returns `404` if the code is unknown.
- URLs are required and limited to 2048 characters. Code collisions during saving are retried up to three times.

To run locally:

- Install .NET 10 SDK, SQL Server and `dotnet-ef` 10.x.
- Set `ConnectionStrings:DefaultConnection` to your database. The default uses Windows LocalDB.
- The migration for the latest model changes is still pending. From the project folder, run:

```bash
dotnet ef migrations add UpdateUrlMappings
dotnet ef database update
dotnet run --launch-profile https
```

Example request:

```http
POST https://localhost:7210/shorten
Content-Type: application/json

{
  "originalUrl": "https://example.com/article"
}
```

Open the returned `shortUrl` to follow the redirect. More request examples are in `UrlShortenerApi.http`.
