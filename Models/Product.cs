using System.ComponentModel.DataAnnotations;

namespace CopilotLabApi.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }

}

public record ProductCreateDto
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string? Name { get; init; }

    [Required]
    [Range(0.0, double.MaxValue)]
    public decimal Price { get; init; }
}

public record ProductUpdateDto
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string? Name { get; init; }

    [Required]
    [Range(0.0, double.MaxValue)]
    public decimal Price { get; init; }
}