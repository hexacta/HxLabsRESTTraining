using FluentValidation;

namespace HxLabsAdvanced.APIService.Models.Validation
{
    //REF 8 Herramienta de validaciones
    public class ActorForCreateDtoValidator : AbstractValidator<ActorForCreateDto>
    {
        public ActorForCreateDtoValidator()
        {
            RuleFor(dto => dto.Name).NotEmpty();

            RuleFor(dto => dto.LastName).NotEmpty();
        }
    }
}
