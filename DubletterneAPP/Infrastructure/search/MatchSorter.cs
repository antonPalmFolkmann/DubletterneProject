namespace Infrastructure.Search{

    public class MatchSorter{
    
        public static List<ISearchAble> SortMatchesKeysHighestScoreFirst(Dictionary<ISearchAble, int> matches){
            var keyValueList = matches.ToList();
            keyValueList.Sort((x, y) => y.Value.CompareTo(x.Value));

            var list = new List<ISearchAble>();
            for (int index = 0; index < keyValueList.Count; index++){
                list.Add(keyValueList[index].Key);
            }
            
            return list;
        }
    }
}