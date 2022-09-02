using X.PagedList;
using System.Text.Json;
using System.Text.Json.Serialization;
using MovieRating.Dto;
using MovieRating.Dal.Data;
using MovieRating.Dal.Data.Models;
using MovieRating.Dal.Services;

namespace MovieRating.Test
{
    public class MovieServiceTests : IDisposable
    {

        private ApplicationDbContext context;
        private MovieService movieService;
        private List<Movie> generatedMovies;
        private static readonly string testUserId = "TestUserId";

        public MovieServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                              .UseInMemoryDatabase("MovieTestDB")
                              .Options;

            context = new ApplicationDbContext(options);
            context.Database.EnsureDeleted();

            generatedMovies = GetTestData();
            context.Movies.AddRange(generatedMovies);
            context.SaveChanges();
            movieService = new MovieService(context);
        }

        [Theory]
        [InlineData(5, null)]
        [InlineData(5, "TestUserId")]

        public async Task GetTopMoviesAsyncTest(int count, string? userId = null)
        {
            var serviceMovies = await movieService.GetTopMoviesAsync(count, userId) as IEnumerable<MovieWithRatingDto>;
            var expectedMovies = generatedMovies.SelectAllMoviesWithRatings(userId)
                                            .OrderByDescending(x => x.AverageRating)
                                            .Take(count);
            Assert.Equal(expectedMovies, serviceMovies);
        }

        [Theory]
        [InlineData(1, null)]
        [InlineData(1, "TestUserId")]
        public async Task GetMovieWithRatingAndActorsAsyncTest(int movieId, string? userId = null)
        {
            var serviceMovie = await movieService.GetMovieWithRatingAndActorsAsync(movieId, userId);
            var expectedMovie = generatedMovies.SelectAllMoviesWithRatingsAndActors(userId)
                                           .First(x => x.Id == movieId);

            Assert.Equal(expectedMovie.UserRating, serviceMovie.UserRating);
        }

        [Theory]
        [InlineData("TestUserId", 1)]
        public async Task GetUserMovieRatingAsyncTest(string userId, int movieId)
        {
            var serviceMovieRating = await movieService.GetUserMovieRatingAsync(userId, movieId);
            var expectedRating = generatedMovies.First(x => x.Id == movieId).Ratings
                                            .First(x => x.UserId == userId);

            Assert.Equal(expectedRating, serviceMovieRating);
        }

        [Theory]
        [InlineData(1, 5, "TestUserId")]
        [InlineData(2, 5, "TestUserId")]
        public async Task GetPagedMoviesWithRatingsAsyncTest(int page, int pageSize, string userId)
        {
            var serviceMovies = await movieService.GetPagedMoviesWithRatingsAsync(page, pageSize, userId) as IEnumerable<MovieWithRatingDto>;
            var expectedMovies = generatedMovies.SelectAllMoviesWithRatings(userId).ToPagedList(page, pageSize) as IEnumerable<MovieWithRatingDto>;

            Assert.Equal(expectedMovies, serviceMovies);
        }

        private static List<Movie> GetTestData()
        {
            var actors = Enumerable.Range(1, 10).Select(actorId => new Actor()
            {
                Id = actorId,
                Name = $"Actor{actorId}",
                Ratings = Enumerable.Range(1, 5).Select(ratingId => new ActorRating
                {
                    Id = actorId * 1000 + ratingId,
                    ActorId = actorId,
                    Rating = ratingId,
                    UserId = ratingId == 1 ? testUserId : $"userRating{ratingId}"
                }).ToList()
            }).ToList();

            return Enumerable.Range(1, 10).Select(movieId => new Movie()
            {
                Id = movieId,
                Title = $"Movie{movieId}",
                Description = $"MovieDescription{movieId}",
                ReleaseDate = DateTime.Parse("30.08.2022"),
                Actors = actors.Take(movieId).ToList(),
                Ratings = Enumerable.Range(1, 5).Select(ratingId => new MovieRatingModel
                {
                    Id = movieId * 1000 + ratingId,
                    MovieId = movieId,
                    Rating = ratingId,
                    UserId = ratingId == 1 ? testUserId : $"userRating{ratingId}"
                }).ToList()
            }).ToList();
        }

        public void Dispose()
        {
            context.Dispose();
        }

    }

    internal static class MovieTestExt
    {
        internal static IEnumerable<MovieWithRatingDto> SelectAllMoviesWithRatings(this IEnumerable<Movie> movies, string? userId = null)
        {
            return movies.Select(x => new MovieWithRatingDto()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ReleaseDate = x.ReleaseDate,
                AverageRating = x.Ratings.Average(r => r.Rating),
                UserRating = x.Ratings.FirstOrDefault(r => r.UserId == userId)?.Rating ?? null
            });
        }
        internal static IEnumerable<MovieWithRatingDto> SelectAllMoviesWithRatingsAndActors(this IEnumerable<Movie> movies, string? userId = null)
        {
            return movies.Select(x => new MovieWithRatingAndActorsDto()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ReleaseDate = x.ReleaseDate,
                AverageRating = x.Ratings.Average(r => r.Rating),
                UserRating = x.Ratings.FirstOrDefault(r => r.UserId == userId)?.Rating,
                Actors = x.Actors.Select(a => new ActorWithRatingDto()
                {
                    Id = a.Id,
                    Name = a.Name,
                    AverageRating = a.Ratings.Average(r => r.Rating),
                    UserRating = a.Ratings.FirstOrDefault(r => r.UserId == userId)?.Rating
                }).ToList()
            });
        }
    }
}