using server.Models;
using server.Models.Domain;

namespace server.Interfaces;

public interface IMovieService
{
    /// <summary>
    /// Returns a list of movies available from both providers with their prices.
    /// </summary>
    Task<ApiResponse<List<MovieComparison>>> GetMoviePriceComparisonsAsync();

    /// <summary>
    /// Returns detailed info for a specific movie by provider and movie ID.
    /// </summary>
    Task<ApiResponse<MovieDetails?>> GetMovieDetailAsync(string provider, string movieId);
}
