namespace Infrastructure.Search{

    public class MatchSorter{
    
        public static List<UserDTO> SortMatchesKeysHighestScoreFirst(Dictionary<UserDTO, int> matches){
            var keyValueList = matches.ToList();
            keyValueList.Sort((x, y) => y.Value.CompareTo(x.Value));

            var list = new List<UserDTO>();
            for (int index = 0; index < keyValueList.Count; index++){
                list.Add(keyValueList[index].Key);
            }
            
            return list;
        }

        public static List<ResourceDTO> SortMatchesKeysHighestScoreFirst(Dictionary<ResourceDTO, int> matches){
            var keyValueList = matches.ToList();
            keyValueList.Sort((x, y) => y.Value.CompareTo(x.Value));

            var list = new List<ResourceDTO>();
            for (int index = 0; index < keyValueList.Count; index++){
                list.Add(keyValueList[index].Key);
            }
            
            return list;
        }
    }
}