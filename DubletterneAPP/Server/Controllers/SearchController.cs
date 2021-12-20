namespace DubletterneAPP.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]



public class SearchController : ControllerBase
{

    private readonly ILogger<SearchController> _logger;

    private readonly IResourceRepository _resourceRepository;

    private readonly IUserRepository _userRepository;

    public SearchController(ILogger<SearchController> logger, IResourceRepository resourceRepository, IUserRepository userRepository)
    {
        _logger = logger;
        _resourceRepository = resourceRepository;
        _userRepository = userRepository;
    }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [HttpGet("{searchParameter}")]
    public async Task<IActionResult> Get(string searchParameter)
    {
        return await Get(searchParameter, "");
    }
    

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [HttpGet("{searchParameter}/{_searchTerm}")]
    public async Task<IActionResult> Get(string searchParameter, string _searchTerm)
    {

        var searchRequestForm = new SearchRequestForm(searchParameter, _searchTerm);

        if (!SearchValidater.ValidateSearchTermCharacters(searchRequestForm.searchTerm)) 
            return NotFound();

        var terms = searchRequestForm.searchTerm.Split(" ");

         return searchRequestForm.searchParam == SearchParam.User ? 
            await getMatches<UserDTO>(terms) : 
            await getMatches<ResourceDTO>(terms);   
    }

    private async Task<IActionResult> getMatches<T>(string[] terms) where T : notnull
    {
        var matchesWithScore = new Dictionary<T, int>();

        foreach (string s in terms)
        {
            IEnumerable<T> matches;

            matches = typeof(T) == typeof(UserDTO) ? 
                matches = (await _userRepository.Search(s)).Cast<T>() : 
                matches = (await _resourceRepository.Search(s)).Cast<T>();

            var i = SearchScorer.ScoreMatch(s);

            foreach (var match in matches)
            {
                if (matchesWithScore.ContainsKey(match))
                    matchesWithScore[match] += i;
                else
                    matchesWithScore.Add(match, i);
            }
        }

        var orderedList = MatchSorter<T>.SortMatchesKeysHighestScoreFirst(matchesWithScore);

        return Ok(orderedList.ToArray());
    }
}