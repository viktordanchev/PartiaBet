using Microsoft.AspNetCore.Http;

namespace Common.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException(string? message = null, int statusCode = StatusCodes.Status500InternalServerError)
            : base(message ?? string.Empty)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; }
    }
}
