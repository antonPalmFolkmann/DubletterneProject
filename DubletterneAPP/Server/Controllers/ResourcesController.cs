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

    [HttpGet("Resources")]
    public IEnumerable<ResourceDTO> Get()
    {
        var list = Enumerable.Range(1, 5).Select(index => new ResourceDTO
        {
            Id = index,
            User = String.Format("User id {0}", index),
            Title = String.Format("Title number {0}", index)
        })
        .ToArray();

        return list;
    }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResourceDetailsDTO), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ResourceDetailsDTO> Get(int id)
        => (await _repository.ReadAsync(id));


}
