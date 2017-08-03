using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HxLabsAdvanced.APIService.Data;
using HxLabsAdvanced.APIService.Entities;

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
        public void AddActorInMovie(Guid movieId, Actor actor)
        {
            var movie = this.GetMovie(movieId);

            if (movie != null)
            {
                if (actor.Id == null)
                {
                    actor.Id = Guid.NewGuid();
                }

                movie.Actors.Add(actor);
            }
        }

        /// <summary>
        /// Check if exist a movie
        /// </summary>
        public bool MovieExists(Guid movieId)
        {
            return this.context.Movies.Any(a => a.Id == movieId);
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
        public Movie GetMovie(Guid movieId)
        {
            return this.context.Movies
                .Where(a => a.Id == movieId)
                .FirstOrDefault();
        }

        /// <summary>
        /// Get all movies
        /// </summary>
        public IEnumerable<Movie> GetMovies()
        {
            return this.context.Movies
                .OrderBy(a => a.DirectorName)
                .ThenBy(a => a.DirectorLastName)
                .ToList();
        }

        /// <summary>
        /// Get movies by ids
        /// </summary>
        public IEnumerable<Movie> GetMovies(IEnumerable<Guid> movieIds)
        {
            return this.context.Movies
                .Where(a => movieIds.Contains(a.Id))
                .OrderBy(a => a.DirectorName)
                .ThenBy(a => a.DirectorLastName)
                .ToList();
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
        public Actor GetActorInMovie(Guid movieId, Guid actorId)
        {
            return this.context.Actors
              .Where(b => b.MovieId == movieId && b.Id == actorId)
              .FirstOrDefault();
        }

        /// <summary>
        /// Get actors in a movie
        /// </summary>
        public IEnumerable<Actor> GetActorsInMovie(Guid movieId)
        {
            return this.context.Actors
                        .Where(b => b.MovieId == movieId)
                        .OrderBy(b => b.Name)
                        .ThenBy(b => b.LastName)
                        .ToList();
        }

        /// <summary>
        /// Update actor in a movie
        /// </summary>
        public void UpdateActorInMovie(Actor actor)
        {
            throw new NotImplementedException("UpdateActorForMovie not implemented");
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
