using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using HxLabsAdvanced.APIService.Helpers.Extensions;

namespace HxLabsAdvanced.APIService.Models.Validation
{
    //REF 8 Herramienta de validaciones
    public class MovieForCreateDtoValidator : AbstractValidator<MovieForCreateDto>
    {
        public MovieForCreateDtoValidator()
        {
            RuleFor(dto => dto.Title).NotEmpty();

            RuleFor(dto => dto.DirectorName).NotEmpty();

            RuleFor(dto => dto.DirectorLastName).NotEmpty();

            RuleFor(dto => dto.Summary).NotEmpty();

            RuleFor(dto => dto.Genre)
                .NotEmpty()
                .WithoutGenreMovies("romance");
        }
    }
}
