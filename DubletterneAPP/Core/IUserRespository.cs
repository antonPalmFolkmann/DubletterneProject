namespace Core
{
    public interface IUserRepository
    {
        Task<(Response, int userId)> CreateAsync(UserCreateDTO user);
        Task<Option<UserDetailsDTO>> ReadAsyncById(int userId);

        Task<Option<UserDetailsDTO>> ReadAsyncByUsername(string username);
        Task<IReadOnlyCollection<UserDTO>> ReadAllAsync();
        Task<Response> UpdateAsync(int userId, UserUpdateDTO user);
        Task<Response> DeleteAsync(int userId);
    }
}