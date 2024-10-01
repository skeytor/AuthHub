using AuthHub.Domain.Entities;
using AuthHub.Domain.Results;
using Microsoft.AspNetCore.Mvc;

namespace AuthHub.Api.Extensions;

public static class ResultExtension
{
    public static ProblemDetails ToProblemDetails(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new NotImplementedException();
        }
        (int statusCode, string title) = GetStatusCodeAndTitle(result.Error.Type);
        return CreateProblemDetails(title, statusCode, result.Error);
    }
    private static (int statusCode, string title) GetStatusCodeAndTitle(ErrorType errorType) =>
    errorType switch
    {
        ErrorType.Validation => (StatusCodes.Status400BadRequest, "Bad Request"),
        ErrorType.NotFound => (StatusCodes.Status404NotFound, "Not Found"),
        ErrorType.Conflict => (StatusCodes.Status409Conflict, "Conflict"),
        _ => (StatusCodes.Status500InternalServerError, "Server Error"),
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
