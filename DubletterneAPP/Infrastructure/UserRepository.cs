
using Core;

namespace Infrastructure
{
    public class UserRepository : IUserRepository
    {
        public Task<(Response, UserDetailsDTO)> CreateAsync(UserCreateDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(int userID)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<UserDTO>> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> ReadAsync(int userID)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(UserDTO user)
        {
            throw new NotImplementedException();
        }
    }
}