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
        public ActorController(IActorService actorService)
        {
            _actorService = actorService;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageNumber = page ?? 1;
            return View("Index", await _actorService.GetPagedActorsWithRatingsAsync(pageNumber, 10, User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
        }

        public async Task<IActionResult> TopActors()
        {
            return View("Index", await _actorService.GetTopActorsAsync(5));
        }

        public async Task<IActionResult> Actor(int actorId)
        {
            return View(await _actorService.GetActorWithRatingAndMoviesAsync(actorId, User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SetActorRating(int rating, int actorId, int? page = 1)
        {
            await _actorService.AddRatingAsync(User.FindFirst(ClaimTypes.NameIdentifier)!.Value, actorId, rating);

            return await Index(page);
        }


    }
}
