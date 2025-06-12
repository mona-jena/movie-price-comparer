namespace server.Models.Domain;

public class MovieSummary
{
    public string Id { get; set; } = null!;
    
    public string Title { get; set; } = null!;
    
    public string? Year { get; set; } = null!;
    
    public string? Poster { get; set; } = null!;
}