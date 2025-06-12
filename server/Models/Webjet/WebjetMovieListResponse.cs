namespace server.Models.Webjet;

public class WebjetMovieListResponse
{
    public List<WebjetMovieSummary> Movies { get; set; } = new();
}

public class WebjetMovieSummary
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Year { get; set; }
    public string? Poster { get; set; }
    public string? Type { get; set; }
}
