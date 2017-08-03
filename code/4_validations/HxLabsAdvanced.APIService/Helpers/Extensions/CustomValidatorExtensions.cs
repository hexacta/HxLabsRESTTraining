using FluentValidation;
using HxLabsAdvanced.APIService.Helpers.Validators;

namespace HxLabsAdvanced.APIService.Helpers.Extensions
{
    public static class CustomValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> WithoutGenreMovies<T>(this IRuleBuilder<T, string> ruleBuilder, string genre)
        {
            return ruleBuilder.SetValidator(new WithoutGenreMoviesValidator(genre));
        }
    }
}
