namespace Core
{
    public interface IResourceRepository
    {
        Task<(Response, ResourceDetailsDTO)> CreateAsync(ResourceCreateDTO resource);
        Task<ResourceDTO> ReadAsync(int resourceID);
        Task<IReadOnlyCollection<ResourceDTO>> ReadAllAsync();
        Task<Response> UpdateAsync(ResourceDTO resource);
        Task<Response> DeleteAsync(int resourceID);
    }
}