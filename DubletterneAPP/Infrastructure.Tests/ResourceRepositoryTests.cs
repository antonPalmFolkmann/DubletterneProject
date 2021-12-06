
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

            var resource1 = new Resource
            {
                Id = 1,
                Title = "Hello, world!",
                User = "UserFuser",
                Created = DateTime.Today,
                TextParagraphs =  new List<TextParagraph>{new TextParagraph("Hello!"), new TextParagraph("How are you?")},
                ImageUrl = "image.com"
            };

            var resource2 = new Resource
            {
                Id = 2,
                Title = "Liberate",
                User = "Animals",
                Created = DateTime.Today,
                TextParagraphs =  new List<TextParagraph>{new TextParagraph("Gutentag"), new TextParagraph("Come stai?")},
                ImageUrl = "image2.com"
            };

            context.AddRange(resource1, resource2);

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
                User = "Bar",
                Created = DateTime.Today,
                TextParagraphs = new List<string> { "string1", "string2", "...", "stringN" },
                ImageUrl = "image.com"
            };

            //Act
            var created = await _repository.CreateAsync(resource);
            
            //Assert
            Assert.Equal(Response.Created, created.Item1);
            Assert.Equal(3, created.Item2);
        }

        [Fact]
        public async void CreateAsync_given_duplicate_Resource_returns_Conflict()
        {
            //Arrange
            var resource = new ResourceCreateDTO
            {
                Title = "Hello, world!",
                User = "UserFuser",
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
                User = "Kappa",
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
                User = "Kappa"
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
                User = "People too",
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
                User = "Animals",
                Created = DateTime.Today,
                TextParagraphs =  new List<TextParagraph>{new TextParagraph("Gutentag"), new TextParagraph("Come stai?")},
                ImageUrl = "image2.com"
            };

            var actual = await _repository.ReadAsync(2);

            Assert.Equal(resource.Id, actual.Value.Id);
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
             resource => Assert.Equal(new ResourceDTO{Id = 1, Title = "Hello, world!", User = "UserFuser"}, resource),
             resource => Assert.Equal(new ResourceDTO{Id = 2, Title = "Liberate", User = "Animals"}, resource)
             );
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}