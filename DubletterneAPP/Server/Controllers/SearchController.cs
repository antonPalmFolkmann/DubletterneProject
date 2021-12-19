using Newtonsoft.Json;
namespace DubletterneAPP.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]



public class SearchController : ControllerBase{

    private readonly ILogger<SearchController> _logger;

    private readonly IResourceRepository _resourceRepository;

    private readonly IUserRepository _userRepository;

    public SearchController(ILogger<SearchController> logger, IResourceRepository resourceRepository, IUserRepository userRepository) {
        _logger = logger;
        _resourceRepository = resourceRepository;
        _userRepository = userRepository;
    }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [HttpGet("{searchParameter}/{_searchTerm}")]
    public async Task<IActionResult> Get(String searchParameter, String _searchTerm){

        var searchRequestForm = new SearchRequestForm(searchParameter, _searchTerm);

        if (!SearchValidater.ValidateSearchTermCharacters(searchRequestForm.searchTerm)) {
                throw new ArgumentException("Search Term is not valid, contains invalid characters.");
        }

        var terms = searchRequestForm.searchTerm.Split(" ");
        var matchesWithScore = new Dictionary<ISearchAble, int>();

        foreach (string s in terms)
            {
                var matches = new List<ISearchAble>();

                switch (searchRequestForm.searchParam){
                    case SearchParam.User:
                        var n = await _userRepository.Search(s);
                        matches = n.ToList<ISearchAble>();
                        break;
                    case SearchParam.Category:
                        break;
                    case SearchParam.Resource:
                        var m = await _resourceRepository.Search(s);
                        matches = m.ToList<ISearchAble>();
                        break;
                }

                var i = SearchScorer.ScoreMatch(s);

                foreach(ISearchAble isa in matches){
                    if (matchesWithScore.ContainsKey(isa)){
                        matchesWithScore[isa] += i;
                    } else {
                        matchesWithScore.Add(isa, i);
                    }
                }
            }

        var orderedList = MatchSorter.SortMatchesKeysHighestScoreFirst(matchesWithScore);
        var z = PagedList<ISearchAble>.ToPagedList(orderedList, searchRequestForm.PageNumber, searchRequestForm.PageSize);
        
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(z.MetaData));

        return Ok(orderedList.ToArray());
    }
}