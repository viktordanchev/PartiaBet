using Microsoft.AspNetCore.Http;

namespace Common.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException(string message, int statusCode = StatusCodes.Status500InternalServerError)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; }
    }
}
