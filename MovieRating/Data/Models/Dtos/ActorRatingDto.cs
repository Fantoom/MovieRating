namespace MovieRating.Data.Models.Dtos
{
    public record ActorRatingDto()
    {
        public string UserId { get; init; } = null!;
        public int ActorId { get; init; }
        public int Rating { get; init; }

        public static ActorRatingDto? MapFrom(ActorRating? actorRating)
        {
            if (actorRating is null)
                return null;

            return new ActorRatingDto { UserId = actorRating.UserId, ActorId = actorRating.ActorId, Rating = actorRating.Rating };
        }
    }
}
