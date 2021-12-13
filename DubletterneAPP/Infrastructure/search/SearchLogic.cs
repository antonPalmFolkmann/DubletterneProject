namespace Infrastructure.Search
{

    public enum SearchParameter{Category, User, Resource}
    public class SearchLogic
    {
        /*
        public record MatchResource{
            public string Title {get; set;}
            public int mathcingPoints {get; set;}
        };

       
        public Dictionary<string, int> matchesWithScore;
        public string[] searchTerm;
        public SearchParameter searchParameter;
        


        public static List<KeyValuePair<string, int>> SearchMAIN(SearchRequestForm searchRequestForm){
            if (!SearchValidater.ValidateSearchTermCharacters(Term)) {
                throw new ArgumentException("Search Term is not valid, contains invalid charecters.");
            }

            var searchTerm = Term.Split(" ");
            var searchParameter = parameter;
            var matchesWithScore = new Dictionary<string, int>();

            switch (searchParameter) {
                case SearchParameter.User:
                    break;
                case SearchParameter.Category:
                    break;
                case SearchParameter.Resource:
                    matchesWithScore = searchResourse(searchTerm);
                    break;
            }

            return sortMatchesHighestValueFirst(matchesWithScore);
        }

        public static List<KeyValuePair<string, int>> sortMatchesHighestValueFirst(Dictionary<string, int> matches){
            var list = matches.ToList();

            list.Sort((x, y) => y.Value.CompareTo(x.Value));

            return list;
        }
        private static int scoreMatch(string searhcword, string searchResult){
            //Not implemented 
            return searhcword.Length * 10;
        }

        private static void searchUser(){
            throw new NotImplementedException();
        }

        private static void searchCatagory(){
            throw new NotImplementedException();
        }

        private static Dictionary<string, int> searchResourse(string[] terms){
            Dictionary<string, int> matches = new Dictionary<string, int>(); 
            foreach (string s in terms)
            {
                var match = queryDatabaseResourseContainingWord(s);
                var matchScore = scoreMatch(s, match);

                if (matches.ContainsKey(match)){
                    matches[match] += matchScore;
                } else {
                    matches.Add(match, matchScore);
                }
            }
            return matches;
        }

        private static string queryDatabaseResourseContainingWord(string s){
            return "DatabaseManagersAPI";
        }

        private static string queryDatabaseCatagoryContainingWord(string s){
            //"DatabaseManagersAPI" call
            return "DatabaseManagersAPI";
        }

        private static string queryDatabaseUsersContainingWord(string s){
            //"DatabaseManagersAPI" call
            return "DatabaseManagersAPI";
        }
*/
        
    }
}