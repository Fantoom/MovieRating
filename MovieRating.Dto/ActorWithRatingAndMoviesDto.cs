namespace MovieRating.Dto
{
    public record ActorWithRatingAndMoviesDto : ActorWithRatingDto
    {
        public List<MovieWithRatingDto> Movies { get; set; } = null!;
    }
}