namespace server.Models.Domain;

public class MovieVersion
{
    public string Id { get; set; } = default!;
    
    public string Provider { get; set; } = default!;
    
    public decimal? Price { get; set; }
    
    public string? Year { get; set; }
    
    public string? Poster { get; set; }
}