using System.ComponentModel.DataAnnotations;

namespace Core
{
    public record UserDTO(int Id, string? GivenName, string? Surname, string? UserName);

    public record UserDetailsDTO(int Id, string? GivenName, string? Surname, string? UserName, string? City, int? FirstAppearance, string? Occupation, string? ImageUrl, IReadOnlySet<string> Powers) : UserDTO(Id, GivenName, Surname, UserName);

    public record UserCreateDTO
{
    [StringLength(50)]
    public string? FirstName { get; init; }

    [StringLength(50)]
    public string? LastName { get; init; }

    [StringLength(50)]
    public string? UserName { get; init; }

    [Range(1900, 2100)]
    public string? Email { get; init; }

    [StringLength(50)]
    public string? PhoneNumber { get; init; }

    [Required]
    public ISet<ResourceDTO> Powers { get; init; } = null!;
}

public record UserUpdateDTO : UserCreateDTO
{
    public int Id { get; init; }
}
}