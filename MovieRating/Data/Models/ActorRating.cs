using System.ComponentModel.DataAnnotations;

namespace MovieRating.Data.Models
{
    public class ActorRating
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public User? User { get; set; } = null!;
        public int ActorId { get; set; }
        public Actor? Actor { get; set; } = null!;
        [Range(0, 10)]
        public int Rating { get; set; }
    }
}
