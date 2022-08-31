namespace MovieRating.Data.Models.Dtos
{
    public record ActorWithRatingDto : ActorDto
    {
        public double? AverageRating { get; set; }
        public int? UserRating;
    }
}
