namespace Core
{
    public interface IResourceRepository
    {
        Task<(Response, int resourceID)> CreateAsync(ResourceCreateDTO resource);
        Task<Option<ResourceDetailsDTO>> ReadAsync(int resourceID);
        Task<IReadOnlyCollection<ResourceDTO>> ReadAllByAuthorAsync(string username);
        Task<IReadOnlyCollection<ResourceDTO>> ReadAllAsync();
        Task<Response> UpdateAsync(int id, ResourceUpdateDTO resource);
        Task<Response> DeleteAsync(int resourceID);

        Task<IEnumerable<ResourceDTO>> Search(string s);
    }
}