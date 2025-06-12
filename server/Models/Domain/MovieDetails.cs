namespace server.Models.Domain;

public class MovieDetails
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Year { get; set; }
    public string? Poster { get; set; }
    public decimal? Price { get; set; }
}