using Core;

namespace Infrastructure
{
    public class ResourceRepository : IResourceRepository
    {
        public Task<(Response, ResourceDetailsDTO)> CreateAsync(ResourceCreateDTO resource)
        {
            throw new NotImplementedException();
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
        
        public Task<ResourceDTO> ReadAsync(int resourceID)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(ResourceDTO resource)
        {
            throw new NotImplementedException();
        }
    }
}