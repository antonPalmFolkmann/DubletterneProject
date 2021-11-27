using System.ComponentModel.DataAnnotations;

namespace Infrastructure
{
    public class Resource
    {
        public int Id { get; init; }

        [StringLength(50)]
        [MinLength(1)]
        [Required]
        public string? Title { get; set; }

        [Required]
        public String? Author { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? Created { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? Updated { get; set; }

        [Required]
        [MinLength(1)]
        public List<string>? TextParagraphs { get; set; }

        [Url]
        [Required]
        public string? ImageUrl { get; set; }
    }
}