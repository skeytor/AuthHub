using AuthHub.Domain.Results;

namespace AuthHub.Domain.Errors;

public static class UserError
{
    public static Error NotFound(Guid userId) => Error.NotFound(
        "User.NotFound", $"The user with the id {userId} was not found.");
    public static Error EmailNotFound(string email) => Error.NotFound(
        "User.EmailNotFound", $"The user with email {email} was not found.");
    public static Error EmailNotUnique() => Error.Conflict(
        "User.EmailNotUnique", $"The email must be unique.");
    public static Error UserNameNotUnique() => Error.Conflict(
        "User.UserName", $"The username must be unique.");
    public static Error EmailAlready(string email) => Error.Validation(
        "User.EmailAlready", $"The email {email} is already.");
    public static Error UserNameAlready(string userName) => Error.Validation(
        "User.UserNameAlready", $"The username {userName} is already.");

}
