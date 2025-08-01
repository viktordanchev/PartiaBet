using Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestAPI.Filters
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
            var statusCode = exception is ApiException apiEx ? 
                apiEx.StatusCode : StatusCodes.Status500InternalServerError;

            context.Result = new ObjectResult(new { error = exception.Message })
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}
