using System.ComponentModel.DataAnnotations;

namespace Acme.BookStore.Status.Dtos;

public class ResourceDto
{
    [Required]
    public int Id { get; set; }

    [MinLength(3)]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class CreateResourceDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class UpdateResourceDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}