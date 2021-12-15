using System.ComponentModel.DataAnnotations;

namespace Core
{
    public record UserDTO : ISearchAble{
        public int Id { get; set; }
        public string? UserName { get; set; }
    }

    public record UserDetailsDTO : UserDTO {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public string? Email { get; set; }
        public ICollection<string>? Resources { get; set; }
    }
    public record UserCreateDTO
    {
        [StringLength(50)]
        [MinLength(1)]
        [Required]
        public string? FirstName { get; set; }

        [StringLength(50)]
        [MinLength(1)]
        [Required]
        public string? LastName { get; set; }

        [StringLength(50)]
        [MinLength(1)]
        [Required]
        public string? UserName { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [EmailAddress]
        [Required]
        public string? Email { get; set; }

    }

    public record UserUpdateDTO : UserCreateDTO
    {
        public int Id { get; init; }
        public DateTime Updated { get; init; }

        public ICollection<string>? Resources { get; set; }
    }
}