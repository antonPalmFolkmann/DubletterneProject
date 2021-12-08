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

    [HttpGet]
    public string Get()
    {
        Console.WriteLine("I got something");
        var list = Enumerable.Range(1, 5).Select(index => new ResourceDTO
        {
            Id = index,
            User = String.Format("User id from server {0}", index),
            Title = String.Format("Title number {0}", index)
        })
        .ToArray();

        var jsonString = JsonSerializer.Serialize(list);
        return jsonString;
    }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResourceDetailsDTO), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ResourceDetailsDTO> Get(int id)
        => (await _repository.ReadAsync(id));


}
