using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HxLabsAdvanced.APIService.Helpers.Attibutes
{
    //REF 8 Herramienta de validaciones
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public virtual IDictionary<string, object> ActionArguments { get; }

        private readonly Type modelType;

        public ValidateModelAttribute(Type modelType)
        {
            this.modelType = modelType;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var model = context.ActionArguments.Values.Where(v => v.GetType() == this.modelType).FirstOrDefault();

            if (model == null)
            {
                context.Result = new BadRequestResult();

                return;
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new ValidationFailedResult(context.ModelState);
            }
        }
    }
}