namespace Infrastructure
{
    public class Resource
    {
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
        public IEnumerable<TextParagraph> TextParagraphs { get; set; } = new List<TextParagraph>();

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