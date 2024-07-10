using System.ComponentModel.DataAnnotations;

namespace AuthHub.Api.Dtos;

public sealed record UserRequest(
    [Required, StringLength(maximumLength:50)]
    string FirstName,

    [Required, StringLength(maximumLength:70)]
    string LastName,

    [Required, StringLength(maximumLength:25)]
    string UserName,

    [Required, StringLength(maximumLength:70), DataType(DataType.EmailAddress), EmailAddress]
    string Email,

    [Required, MinLength(5), DataType(DataType.Password)]
    string Password,

    [Required]
    int RoleId
    );
