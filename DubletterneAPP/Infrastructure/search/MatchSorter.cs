namespace Infrastructure.Search;

public class MatchSorter<T> where T : notnull
{
    public static List<T> SortMatchesKeysHighestScoreFirst(Dictionary<T, int> matches)
    {
        var keyValueList = matches.ToList();
        keyValueList.Sort((x, y) => y.Value.CompareTo(x.Value));

        var list = new List<T>();
        for (int index = 0; index < keyValueList.Count; index++)
        {
            list.Add(keyValueList[index].Key);
        }

        return list;
    }
}