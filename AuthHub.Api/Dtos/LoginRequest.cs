using System.ComponentModel.DataAnnotations;

namespace AuthHub.Api.Dtos;

public sealed record LoginRequest(
    [Required]
    string UserName,

    [Required, DataType(DataType.Password)]
    string Password);