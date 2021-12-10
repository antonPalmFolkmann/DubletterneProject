using Xunit;
using Infrastructure.Search;

namespace Infrastructure.Tests.Search;

    public class SearchLogicTests : IDisposable{
    
    [Theory]
    [InlineData("a", SearchParameter.Resource)]
    public void SearchMain(string input, SearchParameter parameter){
        //Arrange

        //Act

        //Assert
    }

    [Theory]
    [InlineData(new string[]{"A","B","C"}, new int[]{100,300,200}, new int[]{1,2,0})]
    [InlineData(new string[]{"A","B","C","D"}, new int[]{100,200,300,400}, new int[]{3,2,1,0})]
    [InlineData(new string[]{"A","B","C","D","E"}, new int[]{300,100,4000,400,10}, new int[]{2,3,0,1,4})]
    public void sortMatchesHighestValueFirst_sorts_highest_first(string[] keys, int[] values, int[] expectedIndexSort){
        //Arrange
        var dictionaryPreSort = new Dictionary<string, int>();
        for (int i = 0; i < keys.Count(); i++){
            dictionaryPreSort.Add(keys[i], values[i]);
        }

        var expected = new List<KeyValuePair<string, int>>(){};
        foreach (int i in expectedIndexSort){
            expected.Add(new KeyValuePair<string, int>(keys[i], values[i]));
        }

        //Act
        var actually = SearchLogic.sortMatchesHighestValueFirst(dictionaryPreSort);
        

        //Assert
        Assert.Equal(expected, actually);
    }

    [Theory]
    [InlineData()]
    public void scoreMatch_scores_word_based_onLength(){
        //Arrange

        //Act

        //Assert

    }

    [Theory]
    [InlineData()]
    public void scoreMatch_scores_word_based_onX(){
        //Arrange

        //Act

        //Assert

    }

    /*
    private static void searchUser(){

    private static void searchCatagory(){

    private static Dictionary<string, int> searchResourse(string[] terms){
    */

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }
        
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
}