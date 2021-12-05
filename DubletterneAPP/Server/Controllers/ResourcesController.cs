using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using DubletterneAPP.Shared;
using Core;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace DubletterneAPP.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ResourcesController : ControllerBase
{
    private readonly ILogger<ResourcesController> _logger;
    private readonly IResourceRepository _repository;

    public ResourcesController(ILogger<ResourcesController> logger, IResourceRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IEnumerable<ResourceDTO>> Get()
    {
        var listofresources = await Task.Run(() => GetLocallyCreatedResourceDTOs());
        return listofresources;
    }

    private IEnumerable<ResourceDTO> GetLocallyCreatedResourceDTOs()
    {
        var list = Enumerable.Range(1, 5).Select(index => new ResourceDTO
        {
            Id = index,
            User = String.Format("User id {0}", index),
            Title = String.Format("Title number {0}", index)
        })
        .ToArray();

        string jsonString = JsonSerializer.Serialize(list[0]);

        Console.WriteLine(jsonString);
        return list;
    }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResourceDetailsDTO), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ResourceDetailsDTO> Get(int id)
        => (await _repository.ReadAsync(id));


}
