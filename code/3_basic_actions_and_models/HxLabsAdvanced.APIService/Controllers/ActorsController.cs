using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HxLabsAdvanced.APIService.Entities;
using HxLabsAdvanced.APIService.Models;
using HxLabsAdvanced.APIService.Services;
using Microsoft.AspNetCore.Mvc;

namespace HxLabsAdvanced.APIService.Controllers
{
    //REF 7 Crear los verbos básicos y sus comportamientos en el controlador de Actor
    //[Route("api/[controller]")]
    //[Route("api/[controller]/{movieId}/actors")]
    [Route("api/movies/{movieId}/actors")]
    public class ActorsController : Controller
    {
        private readonly ICinemaService cinemaService;

        public ActorsController(ICinemaService cinemaService)
        {
            this.cinemaService = cinemaService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid movieId)
        {
            var movieExist = await this.cinemaService.MovieExists(movieId);

            if (!movieExist)
            {
                return NotFound();
            }

            var actorsInMovieRepo = await this.cinemaService.GetActorsInMovie(movieId);

            var actorsInMovie = Mapper.Map<IEnumerable<ActorDto>>(actorsInMovieRepo);

            return Ok(actorsInMovie);
        }

        [HttpGet("{id}", Name = "GetActorInMovie")]
        public async Task<IActionResult> Get(Guid movieId, Guid id)
        {
            var movieExist = await this.cinemaService.MovieExists(movieId);

            if (!movieExist)
            {
                return NotFound();
            }

            var actorInMovieFromRepo = await this.cinemaService.GetActorInMovie(movieId, id);

            if (actorInMovieFromRepo == null)
            {
                return NotFound();
            }

            var actorInMovie = Mapper.Map<ActorDto>(actorInMovieFromRepo);

            return Ok(actorInMovie);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Guid movieId, [FromBody] ActorForCreateDto actor)
        {
            if (actor == null)
            {
                return BadRequest();
            }

            var movieExist = await this.cinemaService.MovieExists(movieId);

            if (!movieExist)
            {
                return NotFound();
            }

            var actorEntity = Mapper.Map<Actor>(actor);

            //Control comment: MovieId is replaced by the ID that is in the URL
            actorEntity.MovieId = movieId;

            await this.cinemaService.AddActorInMovie(movieId, actorEntity);

            var saveResult = await this.cinemaService.Save();

            if (!saveResult)
            {
                throw new Exception($"Creating a book for author {movieId} failed on save.");
            }

            var actorToReturn = Mapper.Map<ActorDto>(actorEntity);

            return CreatedAtRoute("GetActorInMovie", new { movieId = movieId, id = actorToReturn.Id }, actorToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid movieId, Guid id)
        {
            var movieExists = await this.cinemaService.MovieExists(movieId);

            if (!movieExists)
            {
                return NotFound();
            }

            var actorInMovieRepo = await this.cinemaService.GetActorInMovie(movieId, id);

            if (actorInMovieRepo == null)
            {
                return NotFound();
            }

            this.cinemaService.DeleteActor(actorInMovieRepo);

            var saveResponse = await this.cinemaService.Save();

            if (!saveResponse)
            {
                throw new Exception($"Deleting actor {id} in movie {movieId} failed on save.");
            }

            return NoContent();
        }
    }
}
