using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HxLabsAdvanced.APIService.Helpers.Models
{
    //REF 8 Herramienta de validaciones
    public class ValidationResultModel
    {
        public string Message { get; }

        public List<ValidationError> Errors { get; }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            var errorList = modelState.Keys
                        .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                        .ToList();

            this.Message = "Validation Failed";

            this.Errors = errorList;
        }
    }
}
