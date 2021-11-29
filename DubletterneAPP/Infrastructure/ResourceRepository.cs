using Core;

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
                Author = resource.Author,
                Created = resource.Created, 
                TextParagraphs = resource.TextParagraphs,
                ImageUrl = resource.ImageUrl
            };
            var response = Response.Created;
            
            var resourceDetailsDTO = new ResourceDetailsDTO{
                Id = entity.Id,
                Title = entity.Title,
                Author = entity.Author,
                Created = entity.Created,
                Updated = null,
                TextParagraphs = entity.TextParagraphs,
                ImageUrl = entity.ImageUrl 
            };

            _context.Resources.Add(entity);

            await _context.SaveChangesAsync();

            return (response, resourceDetailsDTO);
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