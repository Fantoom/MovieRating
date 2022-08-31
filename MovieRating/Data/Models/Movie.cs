using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace MovieRating.Data.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public List<MovieRatingModel> Ratings { get; set; } = null!;
        public List<Actor> Actors { get; set; } = null!;
    }

    public record MovieDto()
    {
        public int Id { get; init; }
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public DateTime ReleaseDate { get; init; }

        public static MovieDto MapFrom(Movie movie)
        {
            return new MovieDto { Id = movie.Id, Title = movie.Title, Description = movie.Description, ReleaseDate = movie.ReleaseDate };
        }
    }
}
