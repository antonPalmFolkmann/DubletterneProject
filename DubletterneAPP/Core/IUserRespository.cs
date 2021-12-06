namespace Core
{
    public interface IUserRepository
    {
        Task<(Response, UserDetailsDTO)> CreateAsync(UserCreateDTO user);
        Task<Option<UserDetailsDTO>> ReadAsync(int userId);
        Task<IReadOnlyCollection<UserDTO>> ReadAsync();
        
        //Task<IReadOnlyCollection<UserDTO>> ReadAllByIdAsync();
        Task<Response> UpdateAsync(int userId, UserUpdateDTO user);
        Task<Response> DeleteAsync(int userId);
    }
}