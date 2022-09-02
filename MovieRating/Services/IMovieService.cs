﻿using MovieRating.Data;
using MovieRating.Data.Models;
using MovieRating.Dto;

namespace MovieRating.Services
{
    public interface IMovieService
    {
        Task AddRatingAsync(string userId, int movieId, int rating);
        Task<MovieWithRatingAndActorsDto> GetMovieWithRatingAndActorsAsync(int movieId, string? userId = null);
        Task<AsyncPagedList<MovieWithRatingDto>> GetPagedMoviesWithRatingsAsync(int pageNumber, int pageSize, string? userId = null);
        Task<List<MovieWithRatingDto>> GetTopMoviesAsync(int count, string? userId = null);
        Task<MovieRatingModel?> GetUserMovieRatingAsync(string userId, int movieId);
    }
}