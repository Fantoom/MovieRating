namespace MovieRating.Dto
{
    public record ActorDto()
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
    }
}
