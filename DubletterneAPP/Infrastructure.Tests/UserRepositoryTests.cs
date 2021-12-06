namespace Infrastructure.Tests
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly ILearningContext _context;
        private readonly UserRepository _repository;
        private bool disposeValue;

        public UserRepositoryTests()
        {
            var connection = new SqliteConnection("filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<LearningContext>();
            builder.UseSqlite(connection);
            var context = new LearningContext(builder.Options);
            context.Database.EnsureCreated();

            context.Users.AddRange(
                new User {Id = 1, FirstName = "Harry", LastName = "Potter", UserName = "TBWL", Created = DateTime.Now, Email = "TBWL@diagonal.com"},
                new User {Id = 2, FirstName = "Tom", LastName = "Riddle", UserName = "Voldemort", Created = DateTime.Now, Email = "Voldemort@diagonal.com"},
                new User {Id = 3, FirstName = "Ronald", LastName = "Weasley", UserName = "WeasleyIsKing", Created = DateTime.Now, Email = "TBWL@diagonal.com"},
                new User {Id = 4, FirstName = "Draco", LastName = "Malfoy", UserName = "Ferret", Created = DateTime.Now, Email = "Ferret@diagonal.com"},
                new User {Id = 5, FirstName = "Sirius", LastName = "Black", UserName = "Padfoot", Created = DateTime.Now, Email = "Padfoot@diagonal.com"}
            );

            context.SaveChanges();

            _context = context;
            _repository = new UserRepository(_context);
        }

        [Fact]
        public async void Create_async_new_user_with_generated_Id()
        {
            //Arrange
            var user = new UserCreateDTO
            {
                FirstName = "Remus",
                LastName = "Lupin",
                UserName = "Moony",
                Created = DateTime.Now,
                Email = "Moony@diagonal.com"
            };

            //Act
            var created = await _repository.CreateAsync(user);
            
            //Assert
            Assert.Equal(Response.Created, created.Item1); 
            Assert.Equal("Remus", created.Item2.FirstName);
            Assert.Equal("Lupin", created.Item2.LastName);
            Assert.Equal("Moony", created.Item2.UserName);
            Assert.Equal(DateTime.Now, created.Item2.Created);
            Assert.Equal("Moony@diagonal.com", created.Item2.Email);
        }

        [Fact]
        public async void CreateAsync_given_UserName_Already_Exists_returns_Conflict()
        {
            //Arrange
            var user = new UserCreateDTO
            {
                FirstName = "Harry",
                LastName = "Potter",
                UserName = "TBWL",
                Created = DateTime.Now,
                Email = "TBWL@diagonal.com"
            };

            //Act
            var created = await _repository.CreateAsync(user);
            
            //Assert
            Assert.Equal(Response.Conflict, created.Item1);
        }

        [Fact]
        public async void ReadAsync_returns_all_users()
        {
            //Given
            var users = await _repository.ReadAsync();

            //Then
            Assert.Collection(users,
                user => Assert.Equal(new UserDTO(1, "TBWL"), user),
                user => Assert.Equal(new UserDTO(2, "Voldemort"), user),
                user => Assert.Equal(new UserDTO(2, "WeasleyIsKing"), user),
                user => Assert.Equal(new UserDTO(2, "Ferret"), user),
                user => Assert.Equal(new UserDTO(2, "Padfoot"), user)
            );
        }

        [Fact]
        public async void ReadAsync_given_id_exists_returns_user()
        {
        //Given
        var option = await _repository.ReadAsync(1);

        //When
        var user = option.Value;

        //Then
        Assert.Equal(1, user.Id);
        Assert.Equal("Harry", user.FirstName);
        Assert.Equal("Potter", user.LastName);
        Assert.Equal("TBWL", user.UserName);
        Assert.Equal(DateTime.Now, user.Created);
        Assert.Equal("TBWL@diagonal.com", user.Email);
        }

        [Fact]
        public async void DeleteAsync_user_by_their_id_Return_Deleted()
        {
        //Given
        var deleted = await _repository.DeleteAsync(5);
        
        //Then
        Assert.Equal(Response.Deleted, deleted);
        Assert.Null(await _context.Users.FindAsync(5));
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async void DeleteAsync_userId_not_found_return_NotFound()
        {
        //Given
        var deleted = await _repository.DeleteAsync(10);
        
        //Then
        Assert.Equal(Response.NotFound, deleted);
        }

        [Fact]
        public async void UpdateAsync_given_userId_doesnt_exist_return_NotFound()
        {
        //Given
        var user = new UserUpdateDTO {
            Id = 10,
            FirstName = "Harry",
            LastName = "Potter",
            UserName = "TBWL",
            Created = DateTime.Now,
            Updated = DateTime.Now,
            Email = "Harryotter@diagonal.com"
        };

        //When
        var updated = await _repository.UpdateAsync(10, user);

        //Then
        Assert.Equal(Response.NotFound, updated);
        }
        
        [Fact]
        public async void UpdateAsync_given_userId_exists_return_Updated()
        {
        /*
        //Given
        var user = new UserUpdateDTO {
            Id = 1,
            FirstName = "Harry",
            LastName = "Black",
            UserName = "TBWL",
            Created = DateTime.Now,
            Updated = DateTime.Now,
            Email = "TBWL@diagonal.com",
            Resources = new HashSet<string>()
        };

        //When
        var updated = await _repository.UpdateAsync(user.Id, user);

        //Then
        Assert.Equal(Response.Updated, updated); */
        throw new NotImplementedException();
        }
    }
}