using System.Runtime.CompilerServices;

namespace Infrastructure.Tests.Search;

    public class SearchRequestFormTests : IDisposable{

        /*
        [Fact]
        public void SearchRequestForm_given_non_existing_parameter_returns_exception()
        {
            // Arrange
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act
            new SearchRequestForm("Foo", "Bar");

            // Assert
            Assert.Equal("Failed to parse string to enum type SearchParam\r\n", stringWriter.ToString());
        }
        */

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