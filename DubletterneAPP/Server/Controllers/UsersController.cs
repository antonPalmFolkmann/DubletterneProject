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
    [ProducesResponseType(typeof(UserDetailsDTO), StatusCodes.Status200OK)]
    [HttpGet("{id:int}")]
    public async Task<UserDetailsDTO> Get(int id)
        => (await _repository.ReadAsyncById(id));
        

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UserDetailsDTO), StatusCodes.Status200OK)]
    [HttpGet("{username}")]
    public async Task<UserDetailsDTO> Get(string username)
        => (await _repository.ReadAsyncByUsername(username));


    [HttpPost]
    public async Task<int> Post(UserCreateDTO user)
    {
        Console.WriteLine("I got post request");
        
        var created = await _repository.CreateAsync(user);

        return created.userId;
    }
}