using AuthHub.Domain.Results;

namespace AuthHub.Domain.Errors;

public static class AuthenticationError
{
    public static Error InvalidCredentials => Error.Validation(
        "Auth.InvalidCredentials", $"The credentials are invalid");
}
