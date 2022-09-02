namespace MovieRating.Dto
{
    public record MovieWithRatingAndActorsDto : MovieWithRatingDto
    {
        public List<ActorWithRatingDto> Actors { get; set; } = null!;
    }
}
