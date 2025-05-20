using Domain.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Utils;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler {
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken) {
        ProblemDetails problemDetails;

        if (exception is ApplicationExceptionBase applicationException) {
            logger.LogError(
                applicationException,
                "Application exception:\nType: {type}\nCode: {code}\nMessage: {message}", 
                applicationException.Type,
                applicationException.Code,
                applicationException.Message);

            problemDetails = Problem(applicationException); 

            if (applicationException.Type == ErrorType.Validation) {
                var validationError = (ValidationException)exception;
                problemDetails.Extensions["errors"] = validationError.Errors;
            }
        } else {
            logger.LogError(
                exception, 
                "Unhandled exception has been occured:\n{Message}", 
                exception.Message);

            problemDetails = InternalServerProblem();
        }

        problemDetails.Instance = httpContext.Request.Path;
        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = problemDetails.Status!.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static ProblemDetails Problem(ApplicationExceptionBase exception) 
        => exception.Type switch {
        ErrorType.Validation => new ProblemDetails {
            Title = "One or more validation errors",
            Detail = null,
            Status = StatusCodes.Status400BadRequest,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1"
        },

        ErrorType.NotFound => new ProblemDetails {
            Title = exception.Code,
            Detail = exception.Message,
            Status = StatusCodes.Status404NotFound,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5"
        },

        ErrorType.Conflict => new ProblemDetails {
            Title = exception.Code,
            Detail = exception.Message,
            Status = StatusCodes.Status409Conflict,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.10"
        },

        _ => InternalServerProblem()
    }; 
    
    private static ProblemDetails InternalServerProblem() {
        var problemDetails = new ProblemDetails {
            Title = "Internal server error",
            Status = StatusCodes.Status500InternalServerError,
        };

        return problemDetails;
    }
}
