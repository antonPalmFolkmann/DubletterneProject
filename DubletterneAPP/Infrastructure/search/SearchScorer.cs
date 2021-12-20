namespace Infrastructure.Search;

public class SearchScorer
{
    //Can be expanded upon if more complex score is required. (Word type)
    public static int ScoreMatch(string searchword) => searchword.Length * 10;
}
