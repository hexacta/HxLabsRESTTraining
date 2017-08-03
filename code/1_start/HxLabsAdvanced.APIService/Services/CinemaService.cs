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

        public bool MovieExists(Guid movieId)
        {
            return this.context.Movies.Any(a => a.Id == movieId);
        }

        public void DeleteMovie(Movie movie)
        {
            this.context.Movies.Remove(movie);
        }

        public void DeleteActor(Actor actor)
        {
            this.context.Actors.Remove(actor);
        }

        public Movie GetMovie(Guid movieId)
        {
            return this.context.Movies
                .Where(a => a.Id == movieId)
                .FirstOrDefault();
        }

        public IEnumerable<Movie> GetMovies()
        {
            return this.context.Movies
                .OrderBy(a => a.DirectorName)
                .ThenBy(a => a.DirectorLastName)
                .ToList();
        }

        public IEnumerable<Movie> GetMovies(IEnumerable<Guid> movieIds)
        {
            return this.context.Movies
                .Where(a => movieIds.Contains(a.Id))
                .OrderBy(a => a.DirectorName)
                .ThenBy(a => a.DirectorLastName)
                .ToList();
        }

        public void UpdateMovie(Movie movie)
        {
            throw new NotImplementedException("UpdateMovie not implemented");
        }

        public Actor GetActorForMovie(Guid movieId, Guid actorId)
        {
            return this.context.Actors
              .Where(b => b.MovieId == movieId && b.Id == actorId)
              .FirstOrDefault();
        }

        public IEnumerable<Actor> GetActorsForMovie(Guid movieId)
        {
            return this.context.Actors
                        .Where(b => b.MovieId == movieId)
                        .OrderBy(b => b.Name)
                        .ThenBy(b => b.LastName)
                        .ToList();
        }

        public void UpdateActorForMovie(Actor actor)
        {
            throw new NotImplementedException("UpdateActorForMovie not implemented");
        }

        public async Task<bool> Save()
        {
            var result = await this.context.SaveChangesAsync() >= 0;

            return result;
        }
    }
}
