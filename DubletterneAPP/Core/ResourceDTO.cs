using System.ComponentModel.DataAnnotations;

namespace Core;
public record ResourceDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public UserDTO User { get; set; }
    public string ImageUrl { get; set; }
}

public record ResourceDetailsDTO : ResourceDTO
{
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public ICollection<string>? TextParagraphs { get; set; }
}
public record ResourceCreateDTO
{
    [Required]
    [StringLength(50)]
    [MinLength(5)]
    public string Title { get; set; }

    [Required]
    public UserDTO User { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Created { get; set; }

    [Required]
    public ICollection<string> TextParagraphs { get; set; } = new List<string>();

    [Url]
    [Required]
    public string ImageUrl { get; set; }
}

public record ResourceUpdateDTO : ResourceCreateDTO
{
    public int Id { get; init; }
    public DateTime? Updated { get; init; }
}