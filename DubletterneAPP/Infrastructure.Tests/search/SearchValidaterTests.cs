using Xunit;
using Infrastructure.Search;

namespace Infrastructure.Tests.Search{

    public class SearchValidaterTests : IDisposable{

    [Theory]
    [InlineData("a")]
    [InlineData("abecat!")]
    [InlineData("Search Tearm !? is S:T;R,A.N)G\n")]
    public void SearchTerm_Is_Valid(string input){

        var result = SearchValidater.ValidateSearchTermCharacters(input);

        Assert.True(result);
    }

    [Theory]
    [InlineData("s$¤")]
    [InlineData("§")]
    [InlineData("½")]
    public void SearchTerm_Is_Not_Valid(string input){

        var result = SearchValidater.ValidateSearchTermCharacters(input);

        Assert.False(result);
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