using System.ComponentModel.DataAnnotations;

namespace Core
{
    public record UserDTO(int Id, string? UserName);

    public record UserDetailsDTO(
        int Id,
        string? FirstName,
        string? LastName,
        string? UserName,
        DateTime? Created,
        DateTime? Updated,
        string? Email,
        ISet<string> Resources) : UserDTO(Id, UserName);

    public record UserCreateDTO
    {
        [StringLength(50)]
        [MinLength(1)]
        [Required]
        public string? FirstName { get; init; }

        [StringLength(50)]
        [MinLength(1)]
        [Required]
        public string? LastName { get; init; }

        [StringLength(50)]
        [MinLength(1)]
        [Required]
        public string? UserName { get; init; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; init; }

        [EmailAddress]
        [Required]
        public string? Email { get; init; }

        public ISet<string> Resources { get; init; } = null!;

    }

    public record UserUpdateDTO : UserCreateDTO
    {
        public int Id { get; init; }
        public DateTime Updated { get; init; }
    }
}