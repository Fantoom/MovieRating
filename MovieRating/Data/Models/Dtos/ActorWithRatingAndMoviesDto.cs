namespace MovieRating.Data.Models.Dtos
{
    public record ActorWithRatingAndMoviesDto : ActorWithRatingDto
    {
        public List<MovieWithRatingDto> Movies { get; set; } = null!;
    }
}