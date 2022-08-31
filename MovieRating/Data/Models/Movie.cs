﻿using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace MovieRating.Data.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public List<MovieRatingModel> Ratings { get; set; } = null!;
        public List<Actor> Actors { get; set; } = null!;
    }
}
