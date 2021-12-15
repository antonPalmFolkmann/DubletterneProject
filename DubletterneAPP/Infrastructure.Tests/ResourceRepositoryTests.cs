namespace Infrastructure.Tests

{
    public class ResourceRepositoryTests : IDisposable
    {
        private readonly ILearningContext _context;
        private readonly ResourceRepository _repository;
        private bool disposeValue;

        public ResourceRepositoryTests()
        {
            var connection = new SqliteConnection("filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<LearningContext>();
            builder.UseSqlite(connection);
            var context = new LearningContext(builder.Options);
            context.Database.EnsureCreated();

            var user1 = new User{
                Id = 1,
                FirstName = "User",
                LastName = "Fuser",
                UserName = "UserFuser",
                Created = DateTime.Now,
                Updated = null,
                Email = "userfuser@itu.dk",
                Resources = null
            };

            var user2 = new User{
                Id = 2,
                FirstName = "A",
                LastName = "Nimals",
                UserName = "Animals",
                Created = DateTime.Now,
                Updated = null,
                Email = "animals@itu.dk",
                Resources = null
            };

            var user3 = new User{
                Id = 3,
                FirstName = "His",
                LastName = "Tory",
                UserName = "History",
                Created = DateTime.Now,
                Updated = null,
                Email = "history@itu.dk",
                Resources = null
            };

            var resource1 = new Resource
            {
                Id = 1,
                Title = "Hello, world!",
                User = user1,
                Created = DateTime.Today,
                TextParagraphs =  new List<TextParagraph>{new TextParagraph("Hello!"), new TextParagraph("How are you?")},
                ImageUrl = "image.com"
            };

            var resource2 = new Resource
            {
                Id = 2,
                Title = "Liberate",
                User = user2,
                Created = DateTime.Today,
                TextParagraphs =  new List<TextParagraph>{new TextParagraph("Gutentag"), new TextParagraph("Come stai?")},
                ImageUrl = "image2.com"
            };

            var resource3 = new Resource
            {
                Id = 3,
                Title = "StarWars",
                User = user3,
                Created = DateTime.Today,
                TextParagraphs =  new List<TextParagraph>{new TextParagraph("Hello There"), new TextParagraph("General Kenobi")},
                ImageUrl = "image3.com"
            };

            context.AddRange(resource1, resource2, resource3);

            context.SaveChanges();

            _context = context;
            _repository = new ResourceRepository(_context);
        }

        [Fact]
        public async void CreateAsync_given_Resource_returns_Created_and_new_Resource_with_generated_ID()
        {
            //Arrange
            var resource = new ResourceCreateDTO
            {
                Title = "Foo",
                User = new UserDTO{
                    Id = 1,
                    UserName = "hjpo"
                },
                Created = DateTime.Today,
                TextParagraphs = new List<string> { "string1", "string2", "...", "stringN" },
                ImageUrl = "image.com"
            };

            //Act
            var created = await _repository.CreateAsync(resource);
            
            //Assert
            Assert.Equal(Response.Created, created.Item1);
            Assert.Equal(4, created.Item2);
        }

        [Fact]
        public async void CreateAsync_given_duplicate_Resource_returns_Conflict()
        {
            //Arrange
            var resource = new ResourceCreateDTO
            {
                Title = "Hello, world!",
                User = new UserDTO{
                    Id = 1,
                    UserName = "hjpo"
                },
                Created = DateTime.Today,
                TextParagraphs = new List<string> { "Hello!", "How are you?" },
                ImageUrl = "image.com"
            };

            //Act
            var created = await _repository.CreateAsync(resource);
            
            //Assert
            Assert.Equal(Response.Conflict, created.Item1);
            Assert.Equal(-1, created.Item2);
        }

        [Fact]
        public async Task DeleteAsync_deletes_and_returns_Deleted()
        {
            //Arrange

            //Act
            var response = await _repository.DeleteAsync(1);
            var entity = await _context.Resources.FindAsync(1);

            //Assert
            Assert.Equal(Response.Deleted, response);
            Assert.Null(entity);
        }

       [Fact]
        public async Task DeleteAsync_returns_NotFound()
        {
            var response = await _repository.DeleteAsync(69);

            Assert.Equal(Response.NotFound, response);
        }
        
        
        [Fact]
        public async Task UpdateAsync_updates_and_returns_Updated()
        {
            var resource = new ResourceUpdateDTO
            {
                Id = 1,
                Updated = DateTime.Now,
                Title = "Keepo",
                Created = DateTime.Today,
                TextParagraphs = new List<string> { "Hello!", "How are you?" },
                ImageUrl = "image.com"
            };

            var response = await _repository.UpdateAsync(1, resource);
            
            Assert.Equal(Response.Updated, response);

        }       
        
        [Fact]
        public async Task UpdateAsync_returns_NotFound()
        {
            var resource = new ResourceUpdateDTO
            {
                Id = 69,
                Updated = DateTime.Now,
                Title = "Keepo",
            };

            var updated = await _repository.UpdateAsync(69, resource);

            Assert.Equal(Response.NotFound, updated);
        }

        [Fact]
        public async Task UpdateAsync_existing_Title_returns_Conflict()
        {
            var resource = new ResourceUpdateDTO
            {
                Id = 2,
                Updated = DateTime.Now,
                Title = "Hello, world!",
                Created = DateTime.Today,
                TextParagraphs = new List<string> { "Hello!", "How are you?" },
                ImageUrl = "image.com"
            };

            var response = await _repository.UpdateAsync(2, resource);

            Assert.Equal(Response.Conflict, response);
        }

        [Fact]
        public async Task ReadAsync_returns_Resource_with_given_Id()
        {
            var resource = new Resource
            {
                Id = 2,
                Title = "Liberate",
                Created = DateTime.Today,
                TextParagraphs =  new List<TextParagraph>{new TextParagraph("Gutentag"), new TextParagraph("Come stai?")},
                ImageUrl = "image2.com"
            };

            var actual = await _repository.ReadAsync(2);
            var paragraphs = actual.Value.TextParagraphs;

            Assert.Equal(resource.Id, actual.Value.Id);
            Assert.Collection(paragraphs,
                resource => Assert.Equal("Gutentag", paragraphs.ElementAt(0)),
                resource => Assert.Equal("Come stai?", paragraphs.ElementAt(1))
            );
        }
        
        [Fact]
        public async Task ReadAsync_returns_None_when_no_Resource_has_given_Id()
        {
            var option = await _repository.ReadAsync(69);
            Assert.True(option.IsNone);
        }
        
        [Fact]
        public async Task ReadAsync_returns_all_Resources()
        {
             var resources = await _repository.ReadAllAsync();

             Assert.Collection(resources, 
             resource => Assert.Equal(new ResourceDTO{Id = 1, Title = "Hello, world!"}, resource),
             resource => Assert.Equal(new ResourceDTO{Id = 2, Title = "Liberate"}, resource),
             resource => Assert.Equal(new ResourceDTO{Id = 3, Title = "StarWars"}, resource)
             );
        }


        [Fact]
        public async void Search_for_Resource_by_title_and_return_Resourse()
        {
            //Arrange 
            var searchTerm = "hello, world!";
            var expected = new List<ResourceDTO>()
            {
                new ResourceDTO{Id = 1, Title = "Hello, world!", User = new UserDTO{Id=1, UserName ="UserFuser"}}
            };
            
            //Act
            var actual = await _repository.Search(searchTerm);

            //Assert
            Assert.Equal(expected, actual); 
        }


        [Fact]
        public async void Search_for_Resource_by_partial_title_and_return_Resource()
        {
            //Arrange 
            var searchTerm = "libe";
            var expected = new List<ResourceDTO>()
            {
                new ResourceDTO{Id = 2, Title = "Liberate", User = new UserDTO{Id=2, UserName ="Animals"}}
            };
            
            //Act
            var actual = await _repository.Search(searchTerm);

            //Assert
            Assert.Equal(expected, actual); 
        }


        [Fact]
        public async void Search_for_resource_with_common_leter_and_return_many_resources()
        {
            //Arrange 
            var searchTerm = "w";
            
            //Act
            var actual = await _repository.Search(searchTerm);
            var actualOrdered = actual.OrderBy(resourse => resourse.Id);



            //Assert
            Assert.Collection(actualOrdered, 
                resource => Assert.Equal(new ResourceDTO{Id = 1, Title = "Hello, world!", User=new UserDTO{Id=1, UserName ="UserFuser"}}, resource),
                resource => Assert.Equal(new ResourceDTO{Id = 3, Title = "StarWars", User=new UserDTO{Id=3, UserName ="History"}}, resource)
            );

        }


        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData("\n")]
        public async void Search_for_resource_by_no_search_term_and_return_all_resources(string input)
        {
            //Arrange 
            var searchTerm = input;

            //Act
            var actual = await _repository.Search(searchTerm);
            var actualOrdered = actual.OrderBy(user => user.Id);

            //Assert
            Assert.Collection(actualOrdered, 
                resource => Assert.Equal(new ResourceDTO{Id = 1, Title = "Hello, world!", User = new UserDTO{Id=1, UserName ="UserFuser"}}, resource),
                resource => Assert.Equal(new ResourceDTO{Id = 2, Title = "Liberate", User = new UserDTO{Id=2, UserName ="Animals"}}, resource),
                resource => Assert.Equal(new ResourceDTO{Id = 3, Title = "StarWars", User = new UserDTO{Id=3, UserName ="History"}}, resource)
            );
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}