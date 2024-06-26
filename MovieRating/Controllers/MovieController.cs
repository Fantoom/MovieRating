﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRating.Dal.Services;
using System.Security.Claims;

namespace MovieRating.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly string? _userId;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
            _userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public async Task<IActionResult> Index(int? page)
        {
            int pageNumber = page ?? 1;
            var result = await _movieService.GetPagedMoviesWithRatingsAsync(pageNumber, 10, _userId);
            return View("Index", result);
        }

        public async Task<IActionResult> TopMovies()
        {
            var result = await _movieService.GetTopMoviesAsync(5, _userId);
            return View("Index", result);
        }

        public async Task<IActionResult> Movie(int movieId)
        {
            return View(await _movieService.GetMovieWithRatingAndActorsAsync(movieId, _userId));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SetMovieRating(int rating, int movieId, int? page = 1)
        {
            await _movieService.AddRatingAsync(_userId!, movieId, rating);
            return await Index(page);
        }
    }
}
