namespace MovieRating.Dto
{
    public record ActorRatingDto()
    {
        public string UserId { get; init; } = null!;
        public int ActorId { get; init; }
        public int Rating { get; init; }
    }
}
