using System.ComponentModel.DataAnnotations;
namespace MovieRating.Dal.Data.Models
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
}
