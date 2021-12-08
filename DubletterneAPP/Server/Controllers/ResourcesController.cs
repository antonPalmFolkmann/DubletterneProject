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
    public async Task<string> Get()
    {
        Console.WriteLine("I got something");
        var list = await _repository.ReadAllAsync();
        var array = list.ToArray();

        var jsonString = JsonSerializer.Serialize(array);
        return jsonString;
    }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResourceDetailsDTO), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ResourceDetailsDTO> Get(int id)
        => (await _repository.ReadAsync(id));


}
