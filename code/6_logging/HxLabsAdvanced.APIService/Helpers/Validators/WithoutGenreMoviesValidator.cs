using FluentValidation.Validators;

namespace HxLabsAdvanced.APIService.Helpers.Validators
{
    //REF 8 Herramienta de validaciones
    public class WithoutGenreMoviesValidator : PropertyValidator
    {
        private readonly string genre;

        public WithoutGenreMoviesValidator(string genre)
           : base("No {genre} movies allowed in {PropertyValue}")
        {
            this.genre = genre;
        }
        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null)
            {
                return true;
            }

            if ((context.PropertyValue as string).ToLower().Contains(this.genre.ToLower()))
            {
                return false;
            }

            return true;
        }
    }
}
