using System.ComponentModel.DataAnnotations;

namespace Core
{
    public record ResourceDTO(int Id, string? GivenName, string? Surname, string? UserName);

    public record ResourceDetailsDTO(int Id, string? GivenName, string? Surname, string? UserName, string? City, int? FirstAppearance, string? Occupation, string? ImageUrl, IReadOnlySet<string> Powers) : ResourceDTO(Id, GivenName, Surname, UserName);

    public record ResourceCreateDTO
{
    [StringLength(50)]
    public string? GivenName { get; init; }

    [StringLength(50)]
    public string? Surname { get; init; }

    [StringLength(50)]
    public string? UserName { get; init; }

    [Range(1900, 2100)]
    public int? FirstAppearance { get; init; }

    [StringLength(50)]
    public string? Occupation { get; init; }

    public string? City { get; init; }

    [Required]
    public ISet<string> Powers { get; init; } = null!;
}

public record ResourceUpdateDTO : ResourceCreateDTO
{
    public int Id { get; init; }
}
}