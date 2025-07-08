using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace UserService.Exceptions
{
    public class GlobalMiddlewareHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";


            var (statusCode, message) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
                BadRequestException => (StatusCodes.Status400BadRequest, "Bad request"),
                _ => (StatusCodes.Status500InternalServerError, "Internal server error")
            };

            // 3. Write the error response (RFC 7807 Problem Details)
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = statusCode,
                Title = message,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            }, cancellationToken);


            return true;  // Exception is "handled"
        }
    }
}
