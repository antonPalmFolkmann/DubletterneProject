namespace Core
{
    public interface IResourceRepository
    {
        Task<(Response, int resourceID)> CreateAsync(ResourceCreateDTO resource);
        Task<ResourceDetailsDTO> ReadAsync(int resourceID);
        Task<IReadOnlyCollection<ResourceDTO>> ReadAllByAuthorAsync(UserDTO user);
        Task<IReadOnlyCollection<ResourceDTO>> ReadAsync();
        Task<Response> UpdateAsync(ResourceDTO resource);
        Task<Response> DeleteAsync(int resourceID);
    }
}