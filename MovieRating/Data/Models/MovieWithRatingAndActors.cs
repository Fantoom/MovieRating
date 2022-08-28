namespace MovieRating.Data.Models
{
    public class MovieWithRatingAndActors : MovieWithRating
    {
        public List<ActorWithRating> Actors { get; set; } = null!;
    }
}
