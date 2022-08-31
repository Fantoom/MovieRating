using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MovieRating.Data.Models
{
    public class MovieRatingModel
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public User? User { get; set; } = null!;
        public int MovieId { get; set; }
        public Movie? Movie { get; set; } = null!;
        [Range(0, 10)]
        public int Rating { get; set; }
    }

    public record MovieRatingDto
    {
        public string UserId { get; init; } = null!;
        public int MovieId { get; init; }
        public int Rating { get; init; }

        public static MovieRatingDto? MapFrom(MovieRatingModel? movieRating)
        {
            if (movieRating is null)
                return null;

            return new MovieRatingDto { UserId = movieRating.UserId, MovieId = movieRating.MovieId, Rating = movieRating.Rating };
        }
    }
}
