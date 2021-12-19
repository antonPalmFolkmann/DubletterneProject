using Server.Model;

namespace DubletterneAPP.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUserRepository _repository;

    public UsersController(ILogger<UsersController> logger, IUserRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }


    [HttpGet]
    public async Task<IReadOnlyCollection<UserDTO>> Get() 
        => await _repository.ReadAllAsync();

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UserDetailsDTO), StatusCodes.Status200OK)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDetailsDTO>> Get(int id)
        => (await _repository.ReadAsyncById(id)).ToActionResult();
        

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UserDetailsDTO), StatusCodes.Status200OK)]
    [HttpGet("{username}")]
    public async Task<ActionResult<UserDetailsDTO>> Get(string username)
        => (await _repository.ReadAsyncByUsername(username)).ToActionResult();


    [HttpPost]
    [ProducesResponseType(typeof((Response, int)), 201)]
    public async Task<ActionResult> Post(UserCreateDTO user)
    {
        var (response, createdId) = await _repository.CreateAsync(user);
        var tuple = (response, createdId);
        return CreatedAtAction(nameof(Get), tuple.createdId);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int id, [FromBody] UserUpdateDTO user)
            => (await _repository.UpdateAsync(id, user)).ToActionResult();

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
        => (await _repository.DeleteAsync(id)).ToActionResult();
}