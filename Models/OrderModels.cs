namespace CopilotLabApi.Models;

using System;
using System.ComponentModel.DataAnnotations;

public record Order(int Id, int UserId, string ProductName, int Quantity, decimal TotalPrice, DateTime OrderDate);

public record OrderCreateDto
{
    [Required]
    public int UserId { get; init; }

    [Required]
    [StringLength(200)]
    public string? ProductName { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; init; }

    [Required]
    [Range(0.0, double.MaxValue)]
    public decimal TotalPrice { get; init; }
}
