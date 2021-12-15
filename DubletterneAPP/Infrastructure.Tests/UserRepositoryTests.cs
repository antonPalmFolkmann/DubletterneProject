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
                new User {Id = 1, FirstName = "Harry", LastName = "Potter", UserName = "TBWL", Created = DateTime.Today, Email = "TBWL@diagonal.com"},
                new User {Id = 2, FirstName = "Tom", LastName = "Riddle", UserName = "Voldemort", Created = DateTime.Today, Email = "Voldemort@diagonal.com"},
                new User {Id = 3, FirstName = "Ronald", LastName = "Weasley", UserName = "WeasleyIsKing", Created = DateTime.Today, Email = "TBWL@diagonal.com"},
                new User {Id = 4, FirstName = "Draco", LastName = "Malfoy", UserName = "Ferret", Created = DateTime.Today, Email = "Ferret@diagonal.com"},
                new User {Id = 5, FirstName = "Sirius", LastName = "Black", UserName = "Padfoot", Created = DateTime.Today, Email = "Padfoot@diagonal.com"}
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
                Created = DateTime.Today,
                Email = "Moony@diagonal.com"
            };

            //Act
            var created = await _repository.CreateAsync(user);
            
            //Assert
            Assert.Equal(Response.Created, created.Item1); 
            Assert.Equal(6, created.Item2);
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
                Created = DateTime.Today,
                Email = "TBWL@diagonal.com"
            };

            //Act
            var created = await _repository.CreateAsync(user);
            
            //Assert
            Assert.Equal(Response.Conflict, created.Item1);
        }

        [Fact]
        public async void ReadAsyncById_returns_a_given_user()
        {
            //Given
            var option = await _repository.ReadAsyncById(1);
            var user = option.Value;

            //Then
            Assert.Equal(1, user.Id);
            Assert.Equal("Harry", user.FirstName);
            Assert.Equal("Potter", user.LastName);
            Assert.Equal("TBWL", user.UserName);
            Assert.Equal(DateTime.Today, user.Created);
            Assert.Equal(DateTime.Today, DateTime.Today);
            Assert.Equal("TBWL@diagonal.com", user.Email);

        } 

        [Fact]
        public async void ReadAsyncById_given_id_does_not_exist_returns_None()
        {
            //Given
            var option = await _repository.ReadAsyncById(99);

            //Then
            Assert.True(option.IsNone);
        } 

        
        [Fact]
        public async void ReadAllAsync_return_all_users()
        {
        //Given
        var users = await _repository.ReadAllAsync();

        //Then
        Assert.Collection(users,
            user => Assert.Equal(new UserDTO{Id = 1, UserName = "TBWL"}, user),
            user => Assert.Equal(new UserDTO{Id = 2, UserName = "Voldemort"}, user),
            user => Assert.Equal(new UserDTO{Id = 3, UserName = "WeasleyIsKing"}, user),
            user => Assert.Equal(new UserDTO{Id = 4, UserName = "Ferret"}, user),
            user => Assert.Equal(new UserDTO{Id = 5, UserName = "Padfoot"}, user)
            );
        }
        
        
        [Fact]
        public async void UpdateAsync_given_userId_doesnt_exist_return_response_NotFound()
        {
        //Given
        var user = new UserUpdateDTO {
            Id = 10,
            FirstName = "Harry",
            LastName = "Potter",
            UserName = "TBWL",
            Created = DateTime.Today,
            Updated = DateTime.Today,
            Email = "Harryotter@diagonal.com"
        };

        //When
        var updated = await _repository.UpdateAsync(10, user);

        //Then
        Assert.Equal(Response.NotFound, updated);
        }
        
        
        [Fact]
        public async void UpdateAsync_user_where_userId_exists_and_return__response_Updated()
        {
            //Given
            var user = new UserUpdateDTO {
                Id = 1,
                FirstName = "Harry",
                LastName = "Dotter",
                UserName = "TBWL",
                Created = DateTime.Today,
                Updated = DateTime.Today,
                Email = "YouAreAWizard@diagonal.com",
            };

            //When
            var updated = await _repository.UpdateAsync(user.Id, user);
            var UserOne = await _repository.ReadAsyncById(1);
            //Then
            Assert.Equal(Response.Updated, updated); 
            Assert.Equal("Dotter", UserOne.Value.LastName);
            Assert.Equal("YouAreAWizard@diagonal.com", UserOne.Value.Email);
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

        [Fact]
        public async void DeleteAsync_userId_not_found_return_NotFound()
        {
            //Given
            var deleted = await _repository.DeleteAsync(10);

            //Then
            Assert.Equal(Response.NotFound, deleted);
        } 

        [Fact]
        public async void Search_for_user_by_UserName_and_return_User()
        {
            //Arrange 
            var searchTerm = "tbwl";
            var expected = new List<UserDTO>()
            {
                new UserDTO{Id = 1, UserName = "TBWL"}
            };
            
            //Act
            var actual = await _repository.Search(searchTerm);

            //Assert
            Assert.Equal(expected, actual); 
        }

        [Fact]
        public async void Search_for_user_by_no_search_term_and_return_all_Users()
        {
            //Arrange 
            var searchTerm = " ";

            //Act
            var actual = await _repository.Search(searchTerm);
            
            //Assert
            Assert.Collection(actual, 
            user => Assert.Equal(new UserDTO{Id = 1, UserName = "TBWL"}, user),
            user => Assert.Equal(new UserDTO{Id = 2, UserName = "Voldemort"}, user),
            user => Assert.Equal(new UserDTO{Id = 3, UserName = "WeasleyIsKing"}, user),
            user => Assert.Equal(new UserDTO{Id = 4, UserName = "Ferret"}, user),
            user => Assert.Equal(new UserDTO{Id = 5, UserName = "Padfoot"}, user));
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}