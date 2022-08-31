namespace MovieRating.Data.Models.Dtos
{
    public record MovieRatingDto
    {
        public string UserId { get; init; } = null!;
        public int MovieId { get; init; }
        public int Rating { get; init; }

        public static MovieRatingDto? MapFrom(MovieRatingModel? movieRating)
        {
            if (movieRating is null)
                return null;

            return new MovieRatingDto { UserId = movieRating.UserId, MovieId = movieRating.MovieId, Rating = movieRating.Rating };
        }
    }
}
