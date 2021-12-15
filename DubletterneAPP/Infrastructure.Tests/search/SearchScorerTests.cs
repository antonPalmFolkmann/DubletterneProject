using Infrastructure.Search;

namespace Infrastructure.Tests.Search {

    public class SearchScoreTests : IDisposable{

        [Theory]
        [InlineData("abe", 30)]
        [InlineData("gorilla", 70)]
        [InlineData("", 0)]
        public void Takes_searchmactedstring_and_scores_it_correctly(string word, int expected){
            //Arrange

            //Act
            var actual = SearchScorer.ScoreMatch(word);

            //Assert
            Assert.Equal(expected, actual);
        }

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
}