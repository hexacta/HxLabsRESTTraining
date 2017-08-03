using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HxLabsAdvanced.APIService.Entities;
using HxLabsAdvanced.APIService.Helpers;
using HxLabsAdvanced.APIService.Models;
using HxLabsAdvanced.APIService.Services;
using Microsoft.AspNetCore.Mvc;

namespace HxLabsAdvanced.APIService.Controllers
{
    //REF 6 Crear los verbos básicos y sus comportamientos en el controlador de Movie
    //[Route("api/[controller]")]
    [Route("api/movies")]
    public class MoviesController : Controller
    {
        private readonly ICinemaService cinemaService;

        public MoviesController(ICinemaService cinemaService)
        {
            this.cinemaService = cinemaService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var moviesRepo = await this.cinemaService.GetMovies();

            var moviesDto = Mapper.Map<IEnumerable<MovieDto>>(moviesRepo);

            return Ok(moviesDto);
        }

        [HttpGet("{id}", Name = "GetMovie")]
        public async Task<IActionResult>  Get(Guid id)
        {
            var movieRepo = await this.cinemaService.GetMovie(id);

            if (movieRepo == null)
            {
                return NotFound();
            }

            var movieDto = Mapper.Map<MovieDto>(movieRepo);

            return Ok(movieDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]MovieForCreateDto movie)
        {
            if (movie == null)
            {
                return BadRequest();
            }

            //REF 8 Herramienta de validaciones
            if (!ModelState.IsValid)
            {
                return new ValidationFailedResult(ModelState);
            }

            var movieEntityNew = Mapper.Map<Movie>(movie);

            this.cinemaService.AddMovie(movieEntityNew);

            var saveResult = await this.cinemaService.Save();

            if (!saveResult)
            {
                throw new Exception("Creating an movie failed on save.");
            }

            var movieDto = Mapper.Map<MovieDto>(movieEntityNew);

            return CreatedAtRoute("GetMovie", new { id = movieDto.Id }, movieDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var movieRepo = await this.cinemaService.GetMovie(id);

            if (movieRepo == null)
            {
                return NotFound();
            }

            this.cinemaService.DeleteMovie(movieRepo);

            var saveResult = await this.cinemaService.Save();

            if (!saveResult)
            {
                throw new Exception($"Deleting author {id} failed on save.");
            }

            return NoContent();
        }
    }
}
