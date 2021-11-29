using System.ComponentModel.DataAnnotations;

namespace Infrastructure
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [MinLength(1)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [MinLength(1)]
        public string? LastName { get; set; }
        
        [Required]
        [StringLength(50)]
        [MinLength(1)]
        public string? UserName { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }
        
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        public ICollection<Resource>? Resources { get; set; }
    }
}