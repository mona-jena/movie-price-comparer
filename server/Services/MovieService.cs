using server.Interfaces;
using server.Models;
using server.Models.Domain;

namespace server.Services;

public class MovieService(IWebjetProvider webjetProvider) : IMovieService
{
    public async Task<ApiResponse<List<MovieComparison>>> GetMoviePriceComparisonsAsync()
    {
        var providers = new List<Providers> { Providers.Cinemaworld, Providers.Filmworld };
        var movieMap = new Dictionary<string, Dictionary<string, string>>();
        var errors = new List<ApiError>();

        foreach (var provider in providers)
        {
            try
            {
                var summaries = await webjetProvider.GetMoviesAsync(provider.ToString());
                foreach (var movie in summaries)
                {
                    if (!movieMap.ContainsKey(movie.Title))
                        movieMap[movie.Title] = new Dictionary<string, string>();

                    movieMap[movie.Title][provider.ToString()] = movie.Id;
                }
            }
            catch (Exception ex)
            {
                errors.Add(new ApiError
                {
                    Service = provider.ToString(),
                    Title = $"Failed to fetch movies from {provider}",
                    StatusCode = 502,
                    Detail = ex.Message
                });
            }
        }

        var comparisons = new List<MovieComparison>();

        foreach (var (title, providerMap) in movieMap)
        {
            var versions = new List<MovieVersion>();

            foreach (var (provider, movieId) in providerMap)
            {
                try
                {
                    var detail = await webjetProvider.GetMovieDetailsAsync(provider, movieId);
                    if (detail != null)
                    {
                        versions.Add(new MovieVersion
                        {
                            Id = detail.Id,
                            Provider = provider,
                            Price = detail.Price,
                            Year = detail.Year,
                            Poster = detail.Poster
                        });
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(new ApiError
                    {
                        Service = provider,
                        Title = $"Failed to fetch details for movie '{title}' from {provider}",
                        StatusCode = 502,
                        Detail = ex.Message
                    });
                }
            }

            var cheapest = versions
                .Where(v => v.Price.HasValue)
                .OrderBy(v => v.Price)
                .FirstOrDefault()?.Provider;

            comparisons.Add(new MovieComparison
            {
                Title = title,
                Versions = versions,
                CheapestProvider = cheapest
            });
        }

        return errors.Count != 0
            ? new ApiResponse<List<MovieComparison>>(comparisons, errors)
            : new ApiResponse<List<MovieComparison>>(comparisons);
    }

    public async Task<ApiResponse<MovieDetails?>> GetMovieDetailAsync(string provider, string movieId)
    {
        try
        {
            var detail = await webjetProvider.GetMovieDetailsAsync(provider, movieId);
            return new ApiResponse<MovieDetails?>(detail);
        }
        catch (Exception ex)
        {
            var errors = new List<ApiError>
            {
                new()
                {
                    Service = provider,
                    Title = $"Failed to fetch movie details from {provider}",
                    StatusCode = 502,
                    Detail = ex.Message
                }
            };
            return new ApiResponse<MovieDetails?>(null, errors);
        }
    }
}
