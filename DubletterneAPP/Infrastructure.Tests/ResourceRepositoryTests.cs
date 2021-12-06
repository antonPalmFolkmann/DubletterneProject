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
                User = "AntonFolkmann",
                Created = DateTime.Today,
                TextParagraphs =  new List<TextParagraph>{new TextParagraph("Hello!"), new TextParagraph("How are you?")},
                ImageUrl = "image.com"
            };

            context.AddAsync(resource1);

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
            Assert.Equal(2, created.Item2);
        }

        [Fact]
        public async void CreateAsync_given_duplicate_Resource_returns_Conflict()
        {
            //Arrange
            var resource = new ResourceCreateDTO
            {
                Title = "Hello, world!",
                User = "AntonFolkmann",
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

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}