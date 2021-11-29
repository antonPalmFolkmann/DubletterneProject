using System.ComponentModel.DataAnnotations;

namespace Core
{
    public record ResourceDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? User { get; set; }
    }

    public record ResourceDetailsDTO : ResourceDTO
    {
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public ICollection<string>? TextParagraphs { get; set; }
        public string? ImageUrl { get; set; }
    }
    public record ResourceCreateDTO
    {
        [StringLength(50)]
        [MinLength(1)]
        [Required]
        public string? Title { get; set; }

        [Required]
        public String? User { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? Created { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<string>? TextParagraphs { get; set; }

        [Url]
        [Required]
        public string? ImageUrl { get; set; }
    }

    public record ResourceUpdateDTO : ResourceCreateDTO
    {
        public int Id { get; init; }
        public DateTime Updated { get; init; }
    }
}