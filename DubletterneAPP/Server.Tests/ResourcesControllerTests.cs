namespace Server.Tests.Controllers;

public class ResourcesControllerTests 
{

    [Fact]
    public async Task Create_creates_Resource()
    {
        // Arrange
        var logger = new Mock<ILogger<ResourcesController>>();
        var toCreate = new ResourceCreateDTO();
        var created = new ResourceDetailsDTO{
            Id = 1,
            Title = "Title number one", 
            Created = DateTime.Today, 
            Updated = null, 
            TextParagraphs = new List<string>{"Hello", "there"}, 
            ImageUrl = "https://images.com/superman.png"};
        var repository = new Mock<IResourceRepository>();
        repository.Setup(m => m.CreateAsync(toCreate)).ReturnsAsync((Response.Created, created.Id));
        var controller = new ResourcesController(logger.Object, repository.Object);

        // Act
        var result = await controller.Post(toCreate) as CreatedAtActionResult;
        var resultvalue = result.Value;

        var expected = (Response.Created,created.Id);
        // Assert
        Assert.Equal(expected.Id, resultvalue);
        Assert.Equal("Get", result?.ActionName);
    }

    [Fact]
    public async Task Get_returns_Resources_from_repo()
    {
        // Given
        var logger = new Mock<ILogger<ResourcesController>>();
        var expected = Array.Empty<ResourceDTO>();
        var repository = new Mock<IResourceRepository>();
        repository.Setup(m => m.ReadAllAsync()).ReturnsAsync(expected);
        var controller = new ResourcesController(logger.Object, repository.Object);
    
        // When
        var actual = await controller.Get();
        
        // Then
        Assert.Equal(expected, actual);
    }
   
    [Fact]
    public async Task Get_given_non_existing_returns_NotFound()
    {
        // Given
        var logger = new Mock<ILogger<ResourcesController>>();
        var repository = new Mock<IResourceRepository>();
        repository.Setup(m => m.ReadAsync(42)).ReturnsAsync(default(ResourceDetailsDTO));
        var controller = new ResourcesController(logger.Object, repository.Object);
    
        // When
        var response = await controller.Get(42);
    
        // Then
        Assert.IsType<NotFoundResult>(response.Result);
    }

    [Fact]
    public async Task Get_given_existing_returns_resource()
    {
        // Arrange
        var logger = new Mock<ILogger<ResourcesController>>();
        var repository = new Mock<IResourceRepository>();
        var resource = new ResourceDetailsDTO{
            Id = 1,
            Title = "Title number one", 
            Created = DateTime.Today, 
            Updated = null, 
            TextParagraphs = new List<string>{"Hello", "there"}, 
            ImageUrl = "https://images.com/superman.png"};
        repository.Setup(m => m.ReadAsync(1)).ReturnsAsync(resource);
        var controller = new ResourcesController(logger.Object, repository.Object);

        // Act
        var response = await controller.Get(1);

        // Assert
        Assert.Equal(resource, response.Value);
    }

     [Fact]
    public async Task Put_updates_Character()
    {
        // Arrange
        var logger = new Mock<ILogger<ResourcesController>>();
        var character = new ResourceUpdateDTO();
        var repository = new Mock<IResourceRepository>();
        repository.Setup(m => m.UpdateAsync(1, character)).ReturnsAsync(Response.Updated);
        var controller = new ResourcesController(logger.Object, repository.Object);

        // Act
        var response = await controller.Put(1, character);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task Put_given_unknown_id_returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<ResourcesController>>();
        var character = new ResourceUpdateDTO();
        var repository = new Mock<IResourceRepository>();
        repository.Setup(m => m.UpdateAsync(1, character)).ReturnsAsync(Response.NotFound);
        var controller = new ResourcesController(logger.Object, repository.Object);

        // Act
        var response = await controller.Put(1, character);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task Delete_given_non_existing_returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<ResourcesController>>();
        var repository = new Mock<IResourceRepository>();
        repository.Setup(m => m.DeleteAsync(42)).ReturnsAsync(Response.NotFound);
        var controller = new ResourcesController(logger.Object, repository.Object);

        // Act
        var response = await controller.Delete(42);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task Delete_given_existing_returns_NoContent()
    {
        // Arrange
        var logger = new Mock<ILogger<ResourcesController>>();
        var repository = new Mock<IResourceRepository>();
        repository.Setup(m => m.DeleteAsync(1)).ReturnsAsync(Response.Deleted);
        var controller = new ResourcesController(logger.Object, repository.Object);

        // Act
        var response = await controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

}