using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HxLabsAdvanced.APIService.Entities;
using HxLabsAdvanced.APIService.Helpers;
using HxLabsAdvanced.APIService.Helpers.Attibutes;
using HxLabsAdvanced.APIService.Models;
using HxLabsAdvanced.APIService.Services;
using Microsoft.AspNetCore.JsonPatch;
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

            //REF 8 Herramienta de validaciones
            if (!ModelState.IsValid)
            {
                return new ValidationFailedResult(ModelState);
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

        [HttpPut("{id}")]
        [ValidateModel(typeof(ActorForUpdateDto))]
        public async Task<IActionResult> Update(Guid movieId, Guid id, [FromBody] ActorForUpdateDto actor)
        {
            var movieExist = await this.cinemaService.MovieExists(movieId);

            if (!movieExist)
            {
                return NotFound();
            }

            var actorInMovieFromRepo = await this.cinemaService.GetActorInMovie(movieId, id);

            if (actorInMovieFromRepo == null)
            {
                //NO upserting
                return NotFound();
            }

            Mapper.Map(actor, actorInMovieFromRepo);

            this.cinemaService.UpdateActorInMovie(actorInMovieFromRepo);

            var saveResult = await this.cinemaService.Save();

            if (!saveResult)
            {
                throw new Exception($"Updating book {id} for author {movieId} failed on save.");
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdate(Guid movieId, Guid id, [FromBody] JsonPatchDocument<ActorForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var movieExist = await this.cinemaService.MovieExists(movieId);

            if (!movieExist)
            {
                return NotFound();
            }

            var actorInMovieFromRepo = await this.cinemaService.GetActorInMovie(movieId, id);

            if (actorInMovieFromRepo == null)
            {
                //NO upserting
                return NotFound();
            }

            var actorToPatch = Mapper.Map<ActorForUpdateDto>(actorInMovieFromRepo);

            patchDoc.ApplyTo(actorToPatch, ModelState);

            TryValidateModel(actorToPatch);

            if (!ModelState.IsValid)
            {
                return new ValidationFailedResult(ModelState);
            }

            Mapper.Map(actorToPatch, actorInMovieFromRepo);

            this.cinemaService.UpdateActorInMovie(actorInMovieFromRepo);

            var saveResult = await this.cinemaService.Save();

            if (!saveResult)
            {
                throw new Exception($"Patching actor {id} for movie {movieId} failed on save.");
            }

            return NoContent();
        }

    }
}
