
    using server.Models.Domain;

    namespace server.Interfaces;

    public interface IWebjetProvider
    {
        Task<IEnumerable<MovieSummary>> GetMoviesAsync(string provider);
        Task<MovieDetails?> GetMovieDetailsAsync(string provider, string id);
    }