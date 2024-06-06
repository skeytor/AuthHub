namespace AuthHub.Domain.Entities
{
    /// <summary>
    /// Represents the Role Entity
    /// </summary>
    public sealed class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
