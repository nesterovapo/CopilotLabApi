namespace CopilotLabApi.Models;

using System;
using System.ComponentModel.DataAnnotations;

public record User(int Id, string Name, string Email, DateTime CreatedAt);

public record UserCreateDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string? Name { get; init; }

    [Required]
    [EmailAddress]
    public string? Email { get; init; }
}

public record UserUpdateDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string? Name { get; init; }

    [Required]
    [EmailAddress]
    public string? Email { get; init; }
}
