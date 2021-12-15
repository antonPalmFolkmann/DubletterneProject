namespace Infrastructure.Search{

    public class SearchScorer{
        public static int ScoreMatch(string searchword){
            return searchword.Length * 10;
        }

    }
}