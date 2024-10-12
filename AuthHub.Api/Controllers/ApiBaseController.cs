using AuthHub.Domain.Results;
using Microsoft.AspNetCore.Mvc;

namespace AuthHub.Api.Controllers;

/// <summary>
/// Represents the base class for all API controllers in the application.
/// Provides common functionality and ensures that derived controllers are treated as API controllers
/// with automatic model validation and binding
/// </summary>
[ApiController]
public abstract class ApiBaseController : ControllerBase
{
    /// <summary>
    /// Handles failure results by returning an appropiate HTTP responses based on the type of error.
    /// </summary>
    /// <param name="result">The result object that contains success or error information.</param>
    /// <returns>
    /// A corresponding HTTP status code based on the error type.
    /// </returns>
    /// <exception cref="InvalidOperationException">Thrown if the result indicate success, as this method
    /// is meant to handle failures only.
    /// </exception>
    protected IActionResult HandleFailure(Result result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            { Error.Type: ErrorType.NotFound } => 
                NotFound(CreateProblemDetails("Not Found", result.Error)),
            { Error.Type: ErrorType.Conflict } => 
                Conflict(CreateProblemDetails("Conflict", result.Error)),
            { Error.Type: ErrorType.Failure or ErrorType.Validation } => 
                BadRequest(CreateProblemDetails("Bad Request", result.Error)),
            _ => Problem(title: "Internal Server Error", statusCode: StatusCodes.Status500InternalServerError, detail: "An unexpected error occurred. Please try again later.")
        };
    private static ProblemDetails CreateProblemDetails(
        string title,
        Error error,
        int? status = null,
        Error[]? errors = null) =>
    errors is not null
    ? new()
    {
        Title = title,
        Type = error.Type.ToString(),
        Status = status,
        Detail = error.Description,
        Extensions = { { nameof(errors), errors } },
    }
    : new()
    {
        Title = title,
        Type = error.Type.ToString(),
        Status = status,
        Detail = error.Description,
        Extensions = { { nameof(errors), new[] { error } } },
    };
}