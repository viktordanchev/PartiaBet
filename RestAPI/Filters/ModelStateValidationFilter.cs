using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static Common.Constants.ErrorMessages;

namespace RestAPI.Filters
{
    public class ModelStateValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new { ServerError = InvalidRequest });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
