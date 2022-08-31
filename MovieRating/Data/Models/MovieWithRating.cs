using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRating.Data.Models
{
    public class MovieWithRating
    {
        public MovieDto Movie { get; set; } = null!;
        public double? AverageRating { get; set; }
        public MovieRatingDto? UserRating;
    }
}
