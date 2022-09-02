namespace MovieRating.Dto
{
    public record MovieRatingDto
    {
        public string UserId { get; init; } = null!;
        public int MovieId { get; init; }
        public int Rating { get; init; }
    }
}
