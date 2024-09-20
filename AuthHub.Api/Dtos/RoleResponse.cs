namespace AuthHub.Api.Dtos;

public record RoleResponse(
    int Id, 
    string Name, 
    string Description, 
    HashSet<string> Permissions);
