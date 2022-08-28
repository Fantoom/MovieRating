using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRating.Data.Models
{
    public class MovieWithRating
    {
        public Movie Movie { get; set; } = null!;
        public double? AverageRating { get; set; }
        public MovieRatingModel? UserRating;
    }
}
