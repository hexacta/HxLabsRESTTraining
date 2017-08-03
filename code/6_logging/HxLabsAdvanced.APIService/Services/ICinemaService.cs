using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HxLabsAdvanced.APIService.Entities;
using HxLabsAdvanced.APIService.Helpers;

namespace HxLabsAdvanced.APIService.Services
{
    public interface ICinemaService
    {
        void AddMovie(Movie movie);

        Task AddActorInMovie(Guid movieId, Actor actor);

        Task<bool> MovieExists(Guid movieId);

        void DeleteMovie(Movie movie);

        void DeleteActor(Actor actor);

        Task<Movie> GetMovie(Guid movieId);

        Task<PagedList<Movie>> GetMovies(MoviesResourceParameters moviesResourceParameters);

        Task<IEnumerable<Movie>> GetMovies(IEnumerable<Guid> movieIds);

        void UpdateMovie(Movie movie);

        Task<Actor> GetActorInMovie(Guid movieId, Guid actorId);

        Task<IEnumerable<Actor>> GetActorsInMovie(Guid movieId);

        void UpdateActorInMovie(Actor actor);

        Task<bool> Save();
    }
}
