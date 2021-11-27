using System.ComponentModel.DataAnnotations;

namespace Core
{
    public record UserDTO
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
    }

    public record UserDetailsDTO : UserDTO
    {
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public DateTime? Created { get; set; }
        public string? Email { get; set; }
        public IReadOnlySet<string> Resources { get; set; } = null!;
    }

    public record UserCreateDTO
    {
        [StringLength(50)]
        [MinLength(1)]
        [Required]
        public string? FirstName { get; init; }

        [StringLength(50)]
        [MinLength(1)]
        [Required]
        public string? Surname { get; init; }

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
    }

    public record UserUpdateDTO : UserCreateDTO
    {
        public int Id { get; init; }
    }
}