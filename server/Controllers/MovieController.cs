using Microsoft.AspNetCore.Mvc;
using server.Interfaces;
using server.Models;
using server.Models.Domain;

namespace server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController(IMovieService movieService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<MovieComparison>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<MovieComparison>>>> GetMoviePriceComparison()
    {
        var result = await movieService.GetMoviePriceComparisonsAsync();
        return Ok(result);
    }

    [HttpGet("{provider}/movie/{id}")]
    [ProducesResponseType(typeof(ApiResponse<MovieDetails?>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<MovieDetails?>>> GetMovieDetail(string provider, string id)
    {
        var response = await movieService.GetMovieDetailAsync(provider, id);

        if (response.Data == null && response.Errors.Count == 0)
        {
            return NotFound();
        }

        return Ok(response);
    }
}
