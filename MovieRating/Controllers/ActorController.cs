using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieRating.Dal.Services;
using System.Security.Claims;

namespace MovieRating.Controllers
{
    public class ActorController : Controller
    {
        private readonly IActorService _actorService;
        private readonly string? _userId;

        public ActorController(IActorService actorService)
        {
            _actorService = actorService;
            _userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageNumber = page ?? 1;
            var result = await _actorService.GetPagedActorsWithRatingsAsync(pageNumber, 10, _userId);
            return View("Index", result);
        }

        public async Task<IActionResult> TopActors()
        {
            var result = await _actorService.GetTopActorsAsync(5, _userId);
            return View("Index", result);
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
