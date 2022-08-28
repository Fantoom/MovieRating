namespace MovieRating.Data.Models
{
    public class ActorWithRating
    {
        public Actor Actor { get; set; } = null!;
        public double? AverageRating { get; set; }
        public ActorRating? UserRating;
    }
}
