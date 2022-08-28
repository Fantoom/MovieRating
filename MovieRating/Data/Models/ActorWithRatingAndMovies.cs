namespace MovieRating.Data.Models
{
    public class ActorWithRatingAndMovies : ActorWithRating
    {
        public List<MovieWithRating> Movies { get; set; } = null!;
    }
}