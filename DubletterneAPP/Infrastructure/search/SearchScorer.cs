namespace Infrastructure.Search{

    public class SearchScorer{
        public static int ScoreMatch(string searhcword){
            return searhcword.Length * 10;
        }

    }
}