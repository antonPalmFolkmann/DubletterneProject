namespace Server.Tests.Controllers;

public class UsersControllerTests {

    [Fact]
    public async Task Get_returns_Users_from_repo()
    {
        // Given
        var logger = new Mock<ILogger<UsersController>>();
        var expected = Array.Empty<UserDTO>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.ReadAllAsync()).ReturnsAsync(expected);
        var controller = new UsersController(logger.Object,repository.Object);
    
        // When
        var actual = await controller.Get();
        
        // Then
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Get_given_existing_returns_user()
    {
        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        var user = new UserDetailsDTO{
            Id = 1,
            FirstName = "Alexander",
            LastName = "Hjelmgaard",
            UserName = "hjel",
            Created = new DateTime(2021,12,08),
            Email = "hjel@itu.dk"};
        repository.Setup(m => m.ReadAsyncById(1)).ReturnsAsync(user);
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var response = await controller.Get(1);

        // Assert
        Assert.Equal(user, response.Value);
    }

    [Fact]
    public async Task Get_given_non_existing_returns_NotFound()
    {
        // Given
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.ReadAsyncById(42)).ReturnsAsync(default(UserDetailsDTO));
        var controller = new UsersController(logger.Object, repository.Object);
    
        // When
        var response = await controller.Get(42);
    
        // Then
        Assert.IsType<NotFoundResult>(response.Result);
    }

    [Fact]
    public async Task Put_updates_User()
    {
        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var user = new UserUpdateDTO();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.UpdateAsync(1, user)).ReturnsAsync(Response.Updated);
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var response = await controller.Put(1, user);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task Put_given_unknown_id_returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var user = new UserUpdateDTO();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.UpdateAsync(1, user)).ReturnsAsync(Response.NotFound);
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var response = await controller.Put(1, user);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task Delete_given_non_existing_returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.DeleteAsync(42)).ReturnsAsync(Response.NotFound);
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var response = await controller.Delete(42);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task Delete_given_existing_returns_NoContent()
    {
        // Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.DeleteAsync(1)).ReturnsAsync(Response.Deleted);
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var response = await controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

}