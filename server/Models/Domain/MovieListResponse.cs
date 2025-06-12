namespace server.Models.Domain;

public class MovieListResponse
{
    public List<MovieSummary> Movies { get; set; } = new();
}