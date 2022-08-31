using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRating.Services;
using System.Security.Claims;

namespace MovieRating.Controllers
{
    public class ActorController : Controller
    {
        private readonly IActorService _actorService;
        private string? _userId;

        public ActorController(IActorService actorService)
        {
            _actorService = actorService;
            _userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageNumber = page ?? 1;
            return View("Index", await _actorService.GetPagedActorsWithRatingsAsync(pageNumber, 10, _userId));
        }

        public async Task<IActionResult> TopActors()
        {
            return View("Index", await _actorService.GetTopActorsAsync(5, _userId));
        }

        public async Task<IActionResult> Actor(int actorId)
        {
            return View(await _actorService.GetActorWithRatingAndMoviesAsync(actorId, _userId));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SetActorRating(int rating, int actorId, int? page = 1)
        {
            await _actorService.AddRatingAsync(_userId!, actorId, rating);

            return await Index(page);
        }


    }
}
