
using System.ComponentModel.DataAnnotations;

namespace Infrastructure
{
    public class Resource
    {
        [Key]
        public int Id { get; init; }

        [StringLength(50)]
        [MinLength(1)]
        [Required]
        public string? Title { get; set; }

        [Required]
        public string? User { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? Created { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Updated { get; set; }

        [Required]
        [MinLength(1)]
        public IEnumerable<TextParagraph>? TextParagraphs { get; set; }

        [Url]
        [Required]
        public string? ImageUrl { get; set; }
    }
}