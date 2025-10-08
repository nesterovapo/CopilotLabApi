using System.ComponentModel.DataAnnotations;

namespace CopilotLabApi.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#000000";
}

public class TagCreateDto
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [RegularExpression(@"^#[0-9A-Fa-f]{6}$")]
    public string Color { get; set; } = "#000000";
}

public class TagUpdateDto
{
    [StringLength(50)]
    public string? Name { get; set; }

    [RegularExpression(@"^#[0-9A-Fa-f]{6}$")]
    public string? Color { get; set; }
}