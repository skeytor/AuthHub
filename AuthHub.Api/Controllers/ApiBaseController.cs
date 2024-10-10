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
    protected IActionResult HandleFailure(Result result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            { Error.Type: ErrorType.NotFound } => NotFound(),
            { Error.Type: ErrorType.Conflict } => Conflict(),
            { Error.Type: ErrorType.Failure or ErrorType.Validation } => BadRequest(),
            _ => Problem()
        };
    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
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