
using System.ComponentModel.DataAnnotations;

namespace Infrastructure
{
    public class Resource
    {
        [Key]
        public int Id { get; init; }

        [Required]
        [StringLength(50)]
        [MinLength(5)]
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
 
        public Resource (string title) {
            Title = title;
        }

        public Resource()
        {
        }
    }
}