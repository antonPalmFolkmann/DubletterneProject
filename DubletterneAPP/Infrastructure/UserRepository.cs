
using System.Data.Entity;
using Core;

namespace Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly ILearningContext _context;
        public UserRepository(ILearningContext context){
            _context = context;
        }
        public async Task<(Response, UserDetailsDTO)> CreateAsync(UserCreateDTO user)
        {
            var CheckUserName = from u in _context.Users
                                where u.UserName == user.UserName
                                select new UserDTO(u.Id, u.UserName) {
                                    Id = u.Id,
                                    UserName = u.UserName
            };

            if (CheckUserName.Count() != 0) {
                return (Response.Conflict, null);
            }

            var entity = new User {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Created = user.Created,
                Email = user.Email,
                Resources = await GetResourcesAsync(user.Resources).ToListAsync()
            };

            _context.Users.Add(entity);
            await _context.SaveChangesAsync();

            return new (Response.Created, new UserDetailsDTO(
                entity.Id,
                entity.FirstName,
                entity.LastName,
                entity.UserName,
                entity.Created,
                entity.Updated,
                entity.Email,
                entity.Resources.Select(u => u.Title).ToHashSet()
            ));
        }

        public async Task<Response> DeleteAsync(int userID)
        {
            var entity = await _context.Users.FindAsync(userID);

            if (entity == null) {
                return Response.NotFound;
            }

            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();

            return Response.Deleted;
        }


        public async Task<Option<UserDetailsDTO>> ReadAsync(int userId)
        { 
            var users = from u in _context.Users
                       where u.Id == userId
                       select new UserDetailsDTO(
                           u.Id,
                           u.FirstName,
                           u.LastName,
                           u.UserName,
                           u.Created,
                           u.Updated,
                           u.Email,
                           u.Resources.Select(u => u.Title).ToHashSet()
                       );

            return await users.FirstOrDefaultAsync();
        } 

        public async Task<IReadOnlyCollection<UserDTO>> ReadAsync() =>
            (await _context.Users
                           .Select(u => new UserDTO(u.Id, u.UserName))
                           .ToListAsync())
                           .AsReadOnly();

        public async Task<Response> UpdateAsync(int userId, UserUpdateDTO user)
        {
            var entity = await _context.Users.FindAsync(userId);                                             
            if (entity == null) {
                return Response.NotFound;
            }

            entity.Id = userId;
            entity.FirstName = user.FirstName;
            entity.LastName = user.LastName;
            entity.UserName = user.UserName;
            entity.Created = user.Created;
            entity.Updated = user.Updated;
            entity.Email = user.Email;
            entity.Resources = await GetResourcesAsync(user.Resources).ToListAsync();

            await _context.SaveChangesAsync();
            
            return Response.Updated;
        }

        private async IAsyncEnumerable<Resource> GetResourcesAsync(IEnumerable<string>? resources)
        {
            var exist = await _context.Resources.Where(r => resources.Contains(r.Title))
                                                .ToDictionaryAsync(r => r.Title);
            {
                foreach (var resource in resources) {
                    yield return exist.TryGetValue(resource, out var r) ? r : new Resource(resource);
                }
            }
        }
    }
}