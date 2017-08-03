using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HxLabsAdvanced.APIService.Data;
using HxLabsAdvanced.APIService.Entities;
using HxLabsAdvanced.APIService.Helpers;
using Microsoft.EntityFrameworkCore;

namespace HxLabsAdvanced.APIService.Services
{
    public class CinemaService: ICinemaService
    {
        private readonly CinemaContext context;

        public CinemaService(CinemaContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Get all movies
        /// </summary>
        public void AddMovie(Movie movie)
        {
            movie.Id = Guid.NewGuid();

            this.context.Movies.Add(movie);

            if (movie.Actors.Any())
            {
                foreach (var actor in movie.Actors)
                {
                    actor.Id = Guid.NewGuid();
                }
            }
        }

        /// <summary>
        /// Add actor in a movie
        /// </summary>
        public async Task AddActorInMovie(Guid movieId, Actor actor)
        {
            var movie = await this.GetMovie(movieId);

            if (movie == null)
            {
                return;
            }

            if (actor.Id == null)
            {
                actor.Id = Guid.NewGuid();
            }

            movie.Actors.Add(actor);
        }

        /// <summary>
        /// Check if exist a movie
        /// </summary>
        public async Task<bool> MovieExists(Guid movieId)
        {
            var exist = await this.context.Movies.AnyAsync(a => a.Id == movieId);

            return exist;
        }

        /// <summary>
        /// Delete a movie
        /// </summary>
        public void DeleteMovie(Movie movie)
        {
            this.context.Movies.Remove(movie);
        }

        /// <summary>
        /// Delete a actor
        /// </summary>
        public void DeleteActor(Actor actor)
        {
            this.context.Actors.Remove(actor);
        }

        /// <summary>
        /// Get a movie
        /// </summary>
        public async Task<Movie> GetMovie(Guid movieId)
        {
            var result = await this.context.Movies
                .Where(a => a.Id == movieId)
                .FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Get all movies
        /// </summary>
        public async Task<PagedList<Movie>> GetMovies(MoviesResourceParameters moviesResourceParameters)
        {
            var result =  this.context.Movies
                .OrderBy(a => a.DirectorName)
                .ThenBy(a => a.DirectorLastName)
                .AsNoTracking()
                .AsQueryable();

            return await PagedList<Movie>.Create(result,
                moviesResourceParameters.PageNumber,
                moviesResourceParameters.PageSize);
        }

        /// <summary>
        /// Get movies by ids
        /// </summary>
        public async Task<IEnumerable<Movie>> GetMovies(IEnumerable<Guid> movieIds)
        {
            return await this.context.Movies
                .Where(a => movieIds.Contains(a.Id))
                .OrderBy(a => a.DirectorName)
                .ThenBy(a => a.DirectorLastName)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Update a movie
        /// </summary>
        public void UpdateMovie(Movie movie)
        {
            throw new NotImplementedException("UpdateMovie not implemented");
        }

        /// <summary>
        /// Get actor in a movie
        /// </summary>
        public async Task<Actor> GetActorInMovie(Guid movieId, Guid actorId)
        {
            var result = await this.context.Actors
              .Where(b => b.MovieId == movieId && b.Id == actorId)
              .FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Get actors in a movie
        /// </summary>
        public async Task<IEnumerable<Actor>> GetActorsInMovie(Guid movieId)
        {
            var result =  await this.context.Actors
                        .Where(b => b.MovieId == movieId)
                        .OrderBy(b => b.Name)
                        .ThenBy(b => b.LastName)
                        .AsNoTracking()
                        .ToListAsync();

            return result;
        }

        /// <summary>
        /// Update actor in a movie
        /// </summary>
        public void UpdateActorInMovie(Actor actor)
        {
            this.context.Actors.Update(actor);
        }

        /// <summary>
        /// Save current context
        /// </summary>
        public async Task<bool> Save()
        {
            var result = await this.context.SaveChangesAsync() >= 0;

            return result;
        }
    }
}
