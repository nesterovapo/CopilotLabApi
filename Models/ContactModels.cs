namespace CopilotLabApi.Models;

using System;
using System.ComponentModel.DataAnnotations;

public record ContactMessage(int Id, string Name, string Email, string Subject, string Message, DateTime CreatedAt);

public class ContactCreateDto
{
    [Required]
    [StringLength(100)]
    public string? Name { get; init; }

    [Required]
    [EmailAddress]
    public string? Email { get; init; }

    [Required]
    [StringLength(150)]
    public string? Subject { get; init; }

    [Required]
    [StringLength(2000)]
    public string? Message { get; init; }
}
