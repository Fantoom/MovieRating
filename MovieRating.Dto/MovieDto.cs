using MovieRating.Data.Models;

namespace MovieRating.Dto
{
    public record MovieDto()
    {
        public int Id { get; init; }
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public DateTime ReleaseDate { get; init; }

        public static MovieDto MapFrom(Movie movie)
        {
            return new MovieDto { Id = movie.Id, Title = movie.Title, Description = movie.Description, ReleaseDate = movie.ReleaseDate };
        }
    }
}
