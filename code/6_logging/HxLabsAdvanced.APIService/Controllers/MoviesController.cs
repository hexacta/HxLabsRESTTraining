using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HxLabsAdvanced.APIService.Entities;
using HxLabsAdvanced.APIService.Helpers;
using HxLabsAdvanced.APIService.Models;
using HxLabsAdvanced.APIService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HxLabsAdvanced.APIService.Controllers
{
    //REF 6 Crear los verbos básicos y sus comportamientos en el controlador de Movie
    //[Route("api/[controller]")]
    [Route("api/movies")]
    public class MoviesController : Controller
    {
        private readonly ICinemaService cinemaService;

        private ILogger<MoviesController> logger;

        private readonly IUrlHelper urlHelper;

        public MoviesController(ICinemaService cinemaService, ILogger<MoviesController> logger, IUrlHelper urlHelper)
        {
            this.cinemaService = cinemaService;

            this.logger = logger;

            this.urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetMovies")]
        public async Task<IActionResult> Get(MoviesResourceParameters moviesResourceParameters)
        {
            //REF 10 Paginación
            var moviesRepo = await this.cinemaService.GetMovies(moviesResourceParameters);

            var previousPageLink = moviesRepo.HasPrevious 
                ? CreateMoviesResourceUri(moviesResourceParameters, ResourceUriType.PreviousPage) 
                : null;

            var nextPageLink = moviesRepo.HasNext
                ? CreateMoviesResourceUri(moviesResourceParameters, ResourceUriType.NextPage)
                : null;

            var paginationMetadata = new
            {
                totalCount = moviesRepo.TotalCount,
                pageSize = moviesRepo.PageSize,
                currentPage = moviesRepo.CurrentPage,
                totalPages = moviesRepo.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            var moviesDto = Mapper.Map<IEnumerable<MovieDto>>(moviesRepo);

            return Ok(moviesDto);
        }

        private string CreateMoviesResourceUri(MoviesResourceParameters moviesResourceParameters, ResourceUriType type)
        {
            //REF 10 Paginación
            if (type == ResourceUriType.PreviousPage)
            {
                return this.urlHelper.Link("GetMovies", new
                {
                    pageNumber = moviesResourceParameters.PageNumber - 1,
                    pageSize = moviesResourceParameters.PageSize
                });
            }

            if (type == ResourceUriType.NextPage)
            {
                return this.urlHelper.Link("GetMovies", new
                {
                    pageNumber = moviesResourceParameters.PageNumber + 1,
                    pageSize = moviesResourceParameters.PageSize
                });
            }

            return this.urlHelper.Link("GetMovies", new
            {
                pageNumber = moviesResourceParameters.PageNumber,
                pageSize = moviesResourceParameters.PageSize
            });
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
            this.logger.LogTrace("Ingreso a crear una pelicula");

            if (movie == null)
            {
                this.logger.LogDebug("El objeto pelicula es nulo");
                
                return BadRequest();
            }

            //REF 8 Herramienta de validaciones
            if (!ModelState.IsValid)
            {
                this.logger.LogDebug("El objeto pelicula es invalido");

                return new ValidationFailedResult(ModelState);
            }

            var movieEntityNew = Mapper.Map<Movie>(movie);

            this.logger.LogDebug($"Se envia agrabar la pelicula {movieEntityNew.Title}");

            this.cinemaService.AddMovie(movieEntityNew);

            var saveResult = await this.cinemaService.Save();

            if (!saveResult)
            {
                this.logger.LogWarning("Ocurrio un problema al grabar la pelicula");

                throw new Exception("Creating an movie failed on save.");
            }

            var movieDto = Mapper.Map<MovieDto>(movieEntityNew);

            this.logger.LogInformation(100, $"Se una nueva pelicula {movieDto.Id} con el titulo {movieDto.Title}.");

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
