namespace Core
{
    public interface IUserRepository
    {
        Task<(Response, UserDetailsDTO)> CreateAsync(UserCreateDTO user);
        Task<UserDTO> ReadAsync(int userID);
        Task<IReadOnlyCollection<UserDTO>> ReadAllAsync();
        Task<Response> UpdateAsync(UserDTO user);
        Task<Response> DeleteAsync(int userID);
    }
}