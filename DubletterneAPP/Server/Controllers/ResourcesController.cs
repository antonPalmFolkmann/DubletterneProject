using Server.Model;

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
    public async Task<IReadOnlyCollection<ResourceDTO>> Get() 
        => await _repository.ReadAllAsync();

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResourceDetailsDTO), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ResourceDetailsDTO>> Get(int id)
        => (await _repository.ReadAsync(id)).ToActionResult();



    [HttpPost]
    [ProducesResponseType(typeof((Response, int)), 201)]
    public async Task<ActionResult> Post(ResourceCreateDTO toCreate)
    {
        var (response, createdId) = await _repository.CreateAsync(toCreate);
        var tuple = (response, createdId);
        return CreatedAtAction(nameof(Get), tuple.createdId);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int id, [FromBody] ResourceUpdateDTO resource)
            => (await _repository.UpdateAsync(id, resource)).ToActionResult();


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
        => (await _repository.DeleteAsync(id)).ToActionResult();

}
