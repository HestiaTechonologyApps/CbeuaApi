using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IServices;

public class ExceptionLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IExceptionLogService exceptionLogService)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Build ExceptionLog entity
            var log = new ExceptionLog
            {
                LoggedAt = DateTime.UtcNow,
                Path = context.Request.Path,
                Method = context.Request.Method,
                QueryString = context.Request.QueryString.ToString(),
                User = context.User?.Identity?.Name,
                Headers = string.Join("; ", context.Request.Headers.Select(h => $"{h.Key}: {h.Value}")),
                ExceptionMessage = ex.Message,
                ExceptionType = ex.GetType().FullName,
                StackTrace = ex.StackTrace,
                Source = ex.Source,
                InnerException = ex.InnerException?.ToString(),
                TraceId = context.TraceIdentifier
            };

            if (log.InnerException == null) {
                log.InnerException = "not available";
            }
            // Save to database
           // await exceptionLogService.LogExceptionAsync(log);

            // Optionally, rethrow or return a generic error response
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." + ex.Message  });
        }
    }
}