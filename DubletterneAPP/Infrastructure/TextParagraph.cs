

namespace Infrastructure
{
    public class TextParagraph
    {
        public int Id { get; set; }
        public string Paragraph { get; set; }

        public TextParagraph(string paragraph)
        {
            Paragraph = paragraph;
        }
    }
}