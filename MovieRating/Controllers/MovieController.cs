using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieRating.Data;
using MovieRating.Services;
using System.Security.Claims;

namespace MovieRating.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        public async Task<IActionResult> Index(int? page)
        {
            int pageNumber = page ?? 1;
            return View("Index", await _movieService.GetPagedMoviesWithRatingsAsync(pageNumber, 10, User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
        }

        public async Task<IActionResult> TopMovies()
        {
            return View("Index", await _movieService.GetTopMoviesAsync(5));
        }

        public async Task<IActionResult> Movie(int movieId)
        {
            return View(await _movieService.GetMovieWithRatingAndActorsAsync(movieId, User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SetMovieRating(int rating, int movieId, int? page = 1)
        {
            await _movieService.AddRatingAsync(User.FindFirst(ClaimTypes.NameIdentifier)!.Value, movieId, rating);

            return await Index(page);
        }

    }
}
