using System.ComponentModel.DataAnnotations;

namespace AuthHub.Api.Dtos;

public sealed record CreateRoleRequest(
    [Required, StringLength(maximumLength:50)]
    string Name,

    [Required, StringLength(maximumLength:70)]
    string Description,

    [Required]
    int[] Permissions
    );
