using AuthHub.Domain.Results;

namespace AuthHub.Domain.Errors;

public static class RoleError
{
    public static Error NotFoundByName(string name) => Error.NotFound(
    "Role.NotFound", $"The role with the name {name} was not found.");
}
