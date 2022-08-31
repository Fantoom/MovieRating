namespace MovieRating.Data.Models.Dtos
{
    public record MovieWithRatingAndActorsDto : MovieWithRatingDto
    {
        public List<ActorWithRatingDto> Actors { get; set; } = null!;
    }
}
