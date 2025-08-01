using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestAPI.Extensions
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var type = context.Exception.Data["Type"]?.ToString();

            int statusCode = type switch
            {
                "Conflict" => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Result = new ObjectResult(new { error = exception.Message })
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}
