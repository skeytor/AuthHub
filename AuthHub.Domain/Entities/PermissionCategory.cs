namespace AuthHub.Domain.Entities;

public class PermissionCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public ICollection<Permission> Permissions { get; set; } = [];
}
