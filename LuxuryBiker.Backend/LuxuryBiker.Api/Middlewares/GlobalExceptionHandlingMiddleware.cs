using LuxuryBiker.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace LuxuryBiker.Api.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly Dictionary<Type, Func<HttpContext, Exception, Task<ProblemDetails>>> _exceptionHandlers;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
            _exceptionHandlers = new()
            {
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
                { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
            };
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                var exceptionType = e.GetType();
                string json;
                ProblemDetails problem;

                if (_exceptionHandlers.ContainsKey(exceptionType))
                {
                    problem = await _exceptionHandlers[exceptionType].Invoke(context, e);
                    json = JsonSerializer.Serialize(problem);
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(json);
                    return;
                }

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                problem = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server Error",
                    Title = "Server Error",
                    Detail = "An internal server has ocurred."
                };

                json = JsonSerializer.Serialize(problem);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
        }

        private Task<ProblemDetails> HandleUnauthorizedAccessException(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.FromResult(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
            });
        }

        private Task<ProblemDetails> HandleForbiddenAccessException(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.FromResult(new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Forbidden",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
            });
        }
    }
}
