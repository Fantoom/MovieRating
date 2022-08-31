﻿namespace MovieRating.Data.Models.Dtos
{
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
