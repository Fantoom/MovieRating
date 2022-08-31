using System.ComponentModel.DataAnnotations;
namespace MovieRating.Data.Models
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public List<Movie> Movies { get; set; } = null!;
        public List<ActorRating> Ratings { get; set; } = null!;
    }
    public record ActorDto()
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public static ActorDto MapFrom(Actor actor)
        {
            return new ActorDto { Id = actor.Id, Name = actor.Name };
        }
    }
}
