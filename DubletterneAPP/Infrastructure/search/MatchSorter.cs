namespace Infrastructure.Search{

    public class MatchSorter{
    
        public static List<KeyValuePair<ISearchAble, int>> SortMatchesHighestScoreFirst(Dictionary<ISearchAble, int> matches){
            var list = matches.ToList();
            list.Sort((x, y) => y.Value.CompareTo(x.Value));

            return list;
        }
    }
}