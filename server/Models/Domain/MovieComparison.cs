namespace server.Models.Domain;

public class MovieComparison
{
    public string Title { get; set; } = null!;
    
    public List<MovieVersion> Versions { get; set; } = new();
    
    public string? CheapestProvider { get; set; }
}