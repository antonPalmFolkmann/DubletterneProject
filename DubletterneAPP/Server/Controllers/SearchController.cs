using System.Linq;
using Newtonsoft.Json;
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
        {
            throw new ArgumentException("Search Term is not valid, contains invalid characters.");
        }

        var terms = searchRequestForm.searchTerm.Split(" ");

        if (searchRequestForm.searchParam == SearchParam.User)
        {
            return await getUserMatches(terms);
        }
        else
        {
            return await getResourceMatches(terms);
        }

        
        //var z = PagedList<ISearchAble>.ToPagedList(orderedList, searchRequestForm.PageNumber, searchRequestForm.PageSize);
        //Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(z.MetaData));
        //return Ok(orderedList.ToArray());
    }

    private async Task<IActionResult> getResourceMatches(string[] terms)
    {
        var matchesWithScore = new Dictionary<ResourceDTO, int>();

        foreach (string s in terms)
        {
            var matches = new List<ResourceDTO>();
            var n = await _resourceRepository.Search(s);
            matches = n.ToList<ResourceDTO>();
            var i = SearchScorer.ScoreMatch(s);

            foreach (var match in matches)
            {
                if (matchesWithScore.ContainsKey(match))
                {
                    matchesWithScore[match] += i;
                }
                else
                {
                    matchesWithScore.Add(match, i);
                }
            }
        }

        var orderedList = MatchSorter.SortMatchesKeysHighestScoreFirst(matchesWithScore);
        foreach (var item in orderedList)
        {
            System.Console.WriteLine(item);
        }

        return Ok(orderedList.ToArray());
    }

    private async Task<IActionResult> getUserMatches(string[] terms)
    {
        var matchesWithScore = new Dictionary<UserDTO, int>();

        foreach (string s in terms)
        {
            var matches = new List<UserDTO>();
            var n = await _userRepository.Search(s);
            matches = n.ToList<UserDTO>();
            var i = SearchScorer.ScoreMatch(s);

            foreach (var match in matches)
            {
                if (matchesWithScore.ContainsKey(match))
                {
                    matchesWithScore[match] += i;
                }
                else
                {
                    matchesWithScore.Add(match, i);
                }
            }
        }

        var orderedList = MatchSorter.SortMatchesKeysHighestScoreFirst(matchesWithScore);

        return Ok(orderedList.ToArray());
    }
}