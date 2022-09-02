namespace MovieRating.Dto
{
    public record ActorWithRatingDto : ActorDto
    {
        public double? AverageRating { get; set; }
        public int? UserRating;
    }
}
