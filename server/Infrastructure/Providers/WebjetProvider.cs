using System.Text.Json;
using server.Interfaces;
using server.Models.Domain;
using server.Models.Webjet;

namespace server.Infrastructure.Providers
{
    public class WebjetProvider(HttpClient httpClient, IConfiguration configuration) : IWebjetProvider
    {
        private readonly string _apiBaseUrl = configuration["ExternalApi:Webjet:BaseUrl"] ?? throw new ArgumentNullException("External Base Url missing");
        private readonly string _apiToken = configuration["ExternalApi:Webjet:Token"] ?? throw new ArgumentNullException("API Token missing");

        public async Task<IEnumerable<MovieSummary>> GetMoviesAsync(string provider)
        {
            var requestUrl = $"{_apiBaseUrl}/{provider}/movies";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("x-access-token", _apiToken); 
            
            try
            {
                var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return new List<MovieSummary>(); 
                }

                var json = await response.Content.ReadAsStringAsync();
                var raw = JsonSerializer.Deserialize<WebjetMovieListResponse>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return raw?.Movies.Select(MapToMovieSummary).ToList() ?? [];
            }
            catch (HttpRequestException ex)
            {
                return new List<MovieSummary>();
            }
        }

        public async Task<MovieDetails?> GetMovieDetailsAsync(string provider, string movieId)
        {
            var requestUrl = $"{_apiBaseUrl}/{provider}/movie/{movieId}";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("x-access-token", _apiToken);

            try
            {
                var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var raw = JsonSerializer.Deserialize<WebjetMovieResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (raw == null) return null;

                return new MovieDetails
                {
                    Id = raw.ID,
                    Title = raw.Title,
                    Year = raw.Year,
                    Poster = raw.Poster,
                    Price = decimal.TryParse(raw.Price, out var parsed) ? parsed : null
                };
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }
        
        private static MovieSummary MapToMovieSummary(WebjetMovieSummary source)
        {
            return new MovieSummary
            {
                Id = source.Id,
                Title = source.Title,
                Year = source.Year,
                Poster = source.Poster
            };
        }

    }
}
