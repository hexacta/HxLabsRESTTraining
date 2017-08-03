using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HxLabsAdvanced.APIService.Entities;

namespace HxLabsAdvanced.APIService.Services
{
    public interface ICinemaService
    {
        void AddMovie(Movie movie);

        void AddActorInMovie(Guid movieId, Actor actor);

        bool MovieExists(Guid movieId);

        void DeleteMovie(Movie movie);

        void DeleteActor(Actor actor);

        Movie GetMovie(Guid movieId);

        IEnumerable<Movie> GetMovies();

        IEnumerable<Movie> GetMovies(IEnumerable<Guid> movieIds);

        void UpdateMovie(Movie movie);

        Actor GetActorInMovie(Guid movieId, Guid actorId);

        IEnumerable<Actor> GetActorsInMovie(Guid movieId);

        void UpdateActorInMovie(Actor actor);

        Task<bool> Save();
    }
}
