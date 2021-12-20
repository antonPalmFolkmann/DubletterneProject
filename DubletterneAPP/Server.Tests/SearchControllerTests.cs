using Infrastructure.Search;

namespace Server.Tests.Controllers;

public class SearchControllerTests
{
    [Fact]
    public async Task Get_Matching_Users_From_Search_Parameter()
    {
        // Arrange
        var toCreate = new UserCreateDTO();
        var created = new UserDetailsDTO{
            Id = 1,
            FirstName = "Harry",
            LastName = "Potter",
            UserName = "hapt",
            Created = DateTime.Today,
            Updated = null,
            Email = "hapt@itu.dk",
            Resources = new List<string>{"I am", "Harry Potter"}};

        var logger = new Mock<ILogger<SearchController>>();
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(m => m.CreateAsync(toCreate)).ReturnsAsync((Response.Created, created.Id));

        var resourceRepository = new Mock<IResourceRepository>();
        var controller = new SearchController(logger.Object, resourceRepository.Object, userRepository.Object);

        // Act;
        var result = await controller.Get("User") as OkObjectResult;

        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.IsType<UserDTO[]>(result.Value);
    }
    
    [Fact]
    public async Task Get_Matching_Resources_From_Search_Parameter()
    {
        // Arrange
        var logger = new Mock<ILogger<SearchController>>();

        var userRepository = new Mock<IUserRepository>();
        var userToCreate = new UserCreateDTO();
        var userCreated = new UserDetailsDTO{
            Id = 1,
            FirstName = "Harry",
            LastName = "Potter",
            UserName = "hapt",
            Created = DateTime.Today,
            Updated = null,
            Email = "hapt@itu.dk",
            Resources = new List<string>{"I am", "Harry Potter"}};
        
        userRepository.Setup(m => m.CreateAsync(userToCreate)).ReturnsAsync((Response.Created, userCreated.Id));
        var toCreate = new ResourceCreateDTO();
        var created = new ResourceDetailsDTO{
            Id = 1,
            Title = "Title number one", 
            User = userCreated,
            Created = DateTime.Today, 
            Updated = null, 
            TextParagraphs = new List<string>{"Hello", "there"}, 
            ImageUrl = "https://images.com/superman.png"};
        var repository = new Mock<IResourceRepository>();
        repository.Setup(m => m.CreateAsync(toCreate)).ReturnsAsync((Response.Created, created.Id));
        
        var controller = new SearchController(logger.Object, repository.Object, userRepository.Object);

        // Act;
        var result = await controller.Get("Resource") as OkObjectResult;
        
        // Assert
        Assert.IsType<ResourceDTO[]>(result.Value);
    }
}

