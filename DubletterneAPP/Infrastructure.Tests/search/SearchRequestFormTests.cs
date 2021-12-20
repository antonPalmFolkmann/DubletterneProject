namespace Infrastructure.Tests.Search;

    public class SearchRequestFormTests : IDisposable{

        //This will most likely never happen since all calls to the constructor are in some way hard coded. 
        [Fact]
        public void SearchRequestForm_given_non_existing_parameter_returns_exception()
        {
            // Arrange
            var srf = new SearchRequestForm("Foo", "Bar");
            
            // Act
            var expected_searchParam = SearchParam.Exception;

            // Assert
            Assert.Equal(expected_searchParam, srf.searchParam);
            
        }

        [Theory]
        [InlineData("User", SearchParam.User)]
        [InlineData("Resource", SearchParam.Resource)]
        public void SearchRequestForm_given_existing_parameter_returns_object(string _searchParam, SearchParam _searchParamEnum)
        {
            // Arrange
            var expected_searchParam = _searchParamEnum;
            var expected_searchTerm = "foo";
            
            // Act
            var actual = new SearchRequestForm(_searchParam, "foo");
        
            // Assert
            Assert.Equal(expected_searchParam, actual.searchParam);
            Assert.Equal(expected_searchTerm, actual.searchTerm);
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