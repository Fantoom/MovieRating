using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieRating.Dal.Data.Models;

namespace MovieRating.Dal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<MovieRatingModel> MovieRatings { get; set; } = null!;
        public DbSet<Actor> Actors { get; set; } = null!;
        public DbSet<ActorRating> ActorRatings { get; set; } = null!;

    }
}