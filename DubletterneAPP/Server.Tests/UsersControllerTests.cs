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
        Assert.Equal(user, response);
    }

}