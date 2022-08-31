namespace MovieRating.Data.Models
{
    public class ActorWithRating
    {
        public ActorDto Actor { get; set; } = null!;
        public double? AverageRating { get; set; }
        public ActorRatingDto? UserRating;
    }
}
