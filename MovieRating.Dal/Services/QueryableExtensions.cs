using MovieRating.Dal.Data.Models;
using MovieRating.Dto;

namespace MovieRating.Dal.Services
{
    internal static class QueryableExtensions
    {
        internal static IQueryable<MovieWithRatingDto> SelectAllMoviesWithRatings(
            this IQueryable<Movie> movies,
            string? userId = null)
        {
            return movies.Select(movie => new MovieWithRatingDto()
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                AverageRating = movie.Ratings.Average(r => r.Rating),
                UserRating = movie.Ratings.Where(r => r.UserId == userId).Select(r => r.Rating as int?).FirstOrDefault()
            });
        }

        internal static IQueryable<MovieWithRatingAndActorsDto> SelectAllMoviesWithRatingsAndActors(
            this IQueryable<Movie> movies, 
            string? userId = null)
        {
            return movies.Select(movie => new MovieWithRatingAndActorsDto()
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                AverageRating = movie.Ratings.Average(r => r.Rating),
                UserRating = movie.Ratings.Where(r => r.UserId == userId).Select(r => r.Rating as int?).FirstOrDefault(),
                Actors = movie.Actors.Select(a => new ActorWithRatingDto()
                {
                    Id = a.Id,
                    Name = a.Name,
                    AverageRating = a.Ratings.Average(r => r.Rating),
                    UserRating = a.Ratings.Where(r => r.UserId == userId).Select(r => r.Rating).FirstOrDefault()
                }).ToList()
            });
        }

        internal static IQueryable<ActorWithRatingDto> SelectAllActorsWithRatings(
            this IQueryable<Actor> actors, 
            string? userId = null)
        {
            return actors.Select(actor => new ActorWithRatingDto()
            {
                Id = actor.Id,
                Name = actor.Name,
                AverageRating = actor.Ratings.Average(r => r.Rating),
                UserRating = actor.Ratings.Where(r => r.UserId == userId).Select(r => r.Rating as int?).FirstOrDefault()
            });
        }

        internal static IQueryable<ActorWithRatingAndMoviesDto> SelectAllActorsWithRatingsAndMovies(
            this IQueryable<Actor> actors,
            string? userId = null)
        {
            return actors.Select(actor => new ActorWithRatingAndMoviesDto()
            {
                Id = actor.Id,
                Name = actor.Name,
                AverageRating = actor.Ratings.Average(r => r.Rating),
                UserRating = actor.Ratings.Where(r => r.UserId == userId).Select(r => r.Rating as int?).FirstOrDefault(),
                Movies = actor.Movies.Select(movie => new MovieWithRatingDto()
                {

                    Id = movie.Id,
                    Title = movie.Title,
                    Description = movie.Description,
                    ReleaseDate = movie.ReleaseDate,
                    AverageRating = movie.Ratings.Average(r => r.Rating),
                    UserRating = movie.Ratings.Where(r => r.UserId == userId).Select(r => r.Rating as int?).FirstOrDefault()
                }).ToList()
            });
        }
    }
}
