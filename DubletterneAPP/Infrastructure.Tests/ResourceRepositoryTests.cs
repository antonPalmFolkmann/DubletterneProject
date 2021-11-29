using Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

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
                TextParagraphs = new HashSet<string> { "string1", "string2", "...", "stringN" },
                ImageUrl = "image.com"
            };

            //Act
            var created = await _repository.CreateAsync(resource);
            
            //Assert
            Assert.Equal(Response.Created, created.Item1);
            Assert.Equal(resource.Title, created.Item2.Title);
            Assert.Equal(resource.User, created.Item2.User);
            Assert.Equal(resource.Created, created.Item2.Created);
            Assert.Equal(resource.TextParagraphs, created.Item2.TextParagraphs);
            Assert.Equal(resource.ImageUrl, created.Item2.ImageUrl);
        }

   

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}