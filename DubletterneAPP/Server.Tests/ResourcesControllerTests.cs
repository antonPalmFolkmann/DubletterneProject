using System;
using System.Threading.Tasks;
using Core;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using DubletterneAPP.Server.Controllers;
using System.Collections;
using System.Collections.Generic;

namespace Server.Tests.Controllers;

public class ResourcesControllerTests 
{
    [Fact]
    public async Task Get_returns_Resources_from_repo()
    {
        // Given
        var logger = new Mock<ILogger<ResourcesController>>();
        var expected = Array.Empty<ResourceDTO>();
        var repository = new Mock<IResourceRepository>();
        repository.Setup(m => m.ReadAsync()).ReturnsAsync(expected);
        var controller = new ResourcesController(logger.Object, repository.Object);
    
        // When
        var actual = await controller.Get();
        IList<ResourceDTO> list = new List<ResourceDTO>();
        foreach (var item in actual)
        {
            list.Add(item);
        }
    
        // Then
        Assert.Collection(list,
            resource => Assert.Equal(new ResourceDTO{
                Id = 1, 
                Title = "Title number 1", 
                User = "User id 1"} , resource),
            resource => Assert.Equal(new ResourceDTO{
                Id = 2, 
                Title = "Title number 2", 
                User = "User id 2"} , resource),
            resource => Assert.Equal(new ResourceDTO{
                Id = 3, 
                Title = "Title number 3", 
                User = "User id 3"} , resource),
                resource => Assert.Equal(new ResourceDTO{
                Id = 4, 
                Title = "Title number 4", 
                User = "User id 4"} , resource),
           resource => Assert.Equal(new ResourceDTO{
                Id = 5, 
                Title = "Title number 5", 
                User = "User id 5"} , resource)
        );
    }
}