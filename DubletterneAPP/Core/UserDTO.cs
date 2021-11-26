using System.ComponentModel.DataAnnotations;

namespace Core
{
    public record UserDTO(int Id, string? GivenName, string? Surname, string? UserName);

    public record UserDetailsDTO(int Id, string? GivenName, string? Surname, string? UserName, string? City, int? FirstAppearance, string? Occupation, string? ImageUrl, IReadOnlySet<string> Powers) : UserDTO(Id, GivenName, Surname, UserName);

    public record UserCreateDTO
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

public record UserUpdateDTO : UserCreateDTO
{
    public int Id { get; init; }
}
}