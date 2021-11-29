using Core;
using System.Linq;


namespace Infrastructure
{
    public class ResourceRepository : IResourceRepository
    {

        private ILearningContext _context;

        public ResourceRepository(ILearningContext context)
        {
            _context = context;
        }

        public async Task<(Response, ResourceDetailsDTO)> CreateAsync(ResourceCreateDTO resource)
        {
            var entity = new Resource
            {
                Title = resource.Title,
                User = resource.User,
                Created = resource.Created, 
                TextParagraphs = GetParagraphs(resource.TextParagraphs).ToList(),
                ImageUrl = resource.ImageUrl
            };
            var response = Response.Created;
            
            var resourceDetailsDTO = new ResourceDetailsDTO{
                Id = entity.Id,
                Title = entity.Title,
                User = entity.User,
                Created = entity.Created,
                Updated = null,
                TextParagraphs = entity.TextParagraphs.Select(p => p.Paragraph).ToList(),
                ImageUrl = entity.ImageUrl 
            };

            _context.Resources.Add(entity);

            await _context.SaveChangesAsync();

            return (response, resourceDetailsDTO);
        }

        private IEnumerable<TextParagraph> GetParagraphs(ICollection<string>? textParagraphs)
        {
            foreach (var paragraph in textParagraphs)
            {
                yield return new TextParagraph(paragraph);
            }
        }

        public Task<Response> DeleteAsync(int resourceID)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<ResourceDTO>> ReadAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<IReadOnlyCollection<ResourceDTO>> ReadAllByAuthorAsync(UserDTO user)
        {
            throw new NotImplementedException();
        }
        
        public Task<ResourceDetailsDTO> ReadAsync(int resourceID)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(ResourceDTO resource)
        {
            throw new NotImplementedException();
        }
    }
}