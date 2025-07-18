using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookService.Application.Extensions
{
    public class GlobalErrorMiddleware: IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            var (statusCode, message) = exception switch
            {
                ArgumentNullException => (StatusCodes.Status400BadRequest, "Missing parameter"),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "You don't have access"),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = statusCode,
                Title = message,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            }, cancellationToken);

            return true;
        }
    }
}
