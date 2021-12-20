using System.IO;
using Infrastructure.Search;

namespace Server.Tests.Controllers;

public class SearchControllerTests
{
    [Fact]
    public async Task Get_Matching_Users_From_Search_Parameter_return_Multible_matches_ranked()
    {
        // Arrange
        var logger = new Mock<ILogger<SearchController>>();
        var resourceRepository = new Mock<IResourceRepository>();
        var userRepository = new Mock<IUserRepository>();
        string[] s = new string[]{"a", "b"};
        var liste1 = new List<UserDTO>(){
                    new UserDTO{Id = 1, UserName = "Aa"},
                    new UserDTO{Id = 2, UserName = "Ba"},
                    new UserDTO{Id = 3, UserName = "Ca"}};
        var liste2 = new List<UserDTO>(){
                    new UserDTO{Id = 2, UserName = "Ba"}};           
        userRepository.Setup(m => m.Search(s[0])).ReturnsAsync(liste1);
        userRepository.Setup(m => m.Search(s[1])).ReturnsAsync(liste2);
        var controller = new SearchController(logger.Object, resourceRepository.Object, userRepository.Object);

        // Act;
        var result = await controller.Get("User", "a b") as OkObjectResult;
        var expected = new UserDTO[]{
            new UserDTO{Id = 2, UserName = "Ba"},
            new UserDTO{Id = 1, UserName = "Aa"},
            new UserDTO{Id = 3, UserName = "Ca"}
        };

        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.IsType<UserDTO[]>(result.Value);
        Assert.Equal(expected, result.Value);
    }
    
    [Fact]
    public async Task Get_Matching_Resources_With_empty_parameter_Returns_Every_Resource_Not_Ranked()
    {
        // Arrange
        var logger = new Mock<ILogger<SearchController>>();
        var resourceRepository = new Mock<IResourceRepository>();
        var userRepository = new Mock<IUserRepository>();
        var liste = new List<ResourceDTO>(){
                    new ResourceDTO{Id = 1, Title = "A"},
                    new ResourceDTO{Id = 2, Title = "B"},
                    new ResourceDTO{Id = 3, Title = "C"},
                    new ResourceDTO{Id = 4, Title = "D"}};        
        resourceRepository.Setup(m => m.Search("")).ReturnsAsync(liste);
        var controller = new SearchController(logger.Object, resourceRepository.Object, userRepository.Object);

        // Act;
        var result = await controller.Get("Resource") as OkObjectResult;
        var expected = new ResourceDTO[]{
            new ResourceDTO{Id = 1, Title = "A"},
            new ResourceDTO{Id = 2, Title = "B"},
            new ResourceDTO{Id = 3, Title = "C"},
            new ResourceDTO{Id = 4, Title = "D"}
        };
        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.IsType<ResourceDTO[]>(result.Value);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public async Task search_With_Invalid_Input_Returns_Not_found(){
        //Arrange
        var logger = new Mock<ILogger<SearchController>>();
        var resourceRepository = new Mock<IResourceRepository>();
        var userRepository = new Mock<IUserRepository>();
        var controller = new SearchController(logger.Object, resourceRepository.Object, userRepository.Object);

        //Act
        var result = await controller.Get("Resource", "¤");
        
        //Assert
        Assert.IsType<NotFoundResult>(result);
    }
}

