
namespace Infrastructure;
public class UserRepository : IUserRepository
{
    private readonly ILearningContext _context;
    public UserRepository(ILearningContext context)
    {
        _context = context;
    }
    public async Task<(Response, int userId)> CreateAsync(UserCreateDTO user)
    {
        var CheckUserName = from u in _context.Users
                            where u.UserName == user.UserName
                            select new UserDTO
                            {
                                Id = u.Id,
                                UserName = u.UserName
                            };

        if (CheckUserName.Count() != 0)
            return (Response.Conflict, -1);

        var entity = new User
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Created = user.Created,
            Email = user.Email
        };

        _context.Users.Add(entity);
        await _context.SaveChangesAsync();

        return new(Response.Created, entity.Id);
    }

    public async Task<Option<UserDetailsDTO>> ReadAsyncById(int userId)
    {
        var users = from u in _context.Users
                    where u.Id == userId
                    select new UserDetailsDTO
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        UserName = u.UserName,
                        Created = u.Created,
                        Updated = u.Updated,
                        Email = u.Email,
                        Resources = u.Resources.Select(u => u.Title).ToList()
                    };

        return await users.FirstOrDefaultAsync();
    }

    public async Task<Option<UserDetailsDTO>> ReadAsyncByUsername(string username)
    {
        var users = from u in _context.Users
                    where u.UserName == username
                    select new UserDetailsDTO
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        UserName = u.UserName,
                        Created = u.Created,
                        Updated = u.Updated,
                        Email = u.Email,
                        Resources = u.Resources.Select(u => u.Title).ToList()
                    };

        return await users.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<UserDTO>> ReadAllAsync()
    {

        var users = (await _context.Users
                        .Select(u => new UserDTO { Id = u.Id, UserName = u.UserName })
                        .OrderBy(u => u.Id).ThenBy(u => u.UserName)
                        .ToListAsync())
                        .AsReadOnly();

        return users;
    }


    public async Task<Response> UpdateAsync(int userId, UserUpdateDTO user)
    {
        var entity = await _context.Users.FindAsync(userId);
        if (entity == null)
            return Response.NotFound;

        entity.Id = userId;
        entity.FirstName = user.FirstName;
        entity.LastName = user.LastName;
        entity.UserName = user.UserName;
        entity.Created = user.Created;
        entity.Updated = user.Updated;
        entity.Email = user.Email;

        var existing = await _context.Resources.Where(r => user.Resources.Contains(r.Title)).ToListAsync();

        entity.Resources = existing;

        await _context.SaveChangesAsync();

        return Response.Updated;


    }

    public async Task<Response> DeleteAsync(int userID)
    {
        var entity = await _context.Users.FindAsync(userID);

        if (entity == null)
            return Response.NotFound;

        _context.Users.Remove(entity);
        await _context.SaveChangesAsync();

        return Response.Deleted;
    }

    public async Task<IEnumerable<UserDTO>> Search(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            var users = (await _context.Users
                            .Select(r => new UserDTO { Id = r.Id, UserName = r.UserName })
                            .ToListAsync());
            return users;
        }
        var matches = await _context.Users
                                        .Where(r => r.UserName.ToLower().Contains(s.ToLower()))
                                        .Select(r => new UserDTO { Id = r.Id, UserName = r.UserName })
                                        .ToListAsync();
        return matches;
    }
}