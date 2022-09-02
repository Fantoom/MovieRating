using Microsoft.AspNetCore.Identity;

namespace MovieRating.Dal.Data.Models
{
    public class User : IdentityUser
    {
        List<MovieRatingModel> MovieRatings { get; set; } = null!;
    }
}
