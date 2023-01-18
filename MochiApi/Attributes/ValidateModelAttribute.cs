using Microsoft.AspNetCore.Mvc.Filters;
using MochiApi.Error;

namespace MochiApi.Attributes
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ValidationFailedResult(context.ModelState);
            }
        }
    }
}
