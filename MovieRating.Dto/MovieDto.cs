namespace MovieRating.Dto
{
    public record MovieDto()
    {
        public int Id { get; init; }
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public DateTime ReleaseDate { get; init; }
    }
}
