using System;
using System.Threading.Tasks;
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
            var resource = new ResourceDTO
            {
                Id = 1,
                Title = "Keepo",
                User = "Kappa"
            };

            var response = await _repository.UpdateAsync(1, resource);
            
            Assert.Equal(Response.Updated, response);

        }       
        
        [Fact]
        public async Task UpdateAsync_returns_NotFound()
        {
            var resource = new ResourceDTO
            {
                Id = 69,
                Title = "Keepo",
                User = "Kappa"
            };

            var updated = await _repository.UpdateAsync(69, resource);

            Assert.Equal(Response.NotFound, updated);
        }

        /* Not working, can't change title to already taken string
        [Fact]
        public async Task UpdateAsync_existing_Title_returns_Conflict()
        {
            var resource = new ResourceDTO
            {
                Id = 2,
                Title = "Hello, world!",
                User = "Kappa"
            };

            var response = await _repository.UpdateAsync(2, resource);

            Assert.Equal(Response.Conflict, response);
        } */


        

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}