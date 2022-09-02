using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRating.Dto
{
    public record MovieWithRatingDto : MovieDto
    {
        public double? AverageRating { get; init; }
        public int? UserRating { get; init; }
    }
}
