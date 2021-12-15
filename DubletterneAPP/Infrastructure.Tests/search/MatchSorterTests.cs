using Infrastructure.Search;

namespace Infrastructure.Tests.Search {

    public class MatchSorterTests : IDisposable{

        [Theory]
        [MemberData(nameof(Data))]
        public void sortMatchesHighestValueFirst_sorts_highest_first(Dictionary<ISearchAble, int> keyValueDictonary, List<ISearchAble> expected){
            //Arrange

            //Act
            var actual = MatchSorter.SortMatchesKeysHighestScoreFirst(keyValueDictonary);

            //Assert
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] { 
                    new Dictionary<ISearchAble, int>(){
                        {new UserDTO{Id = 1, UserName = "A"}, 10},
                        {new UserDTO{Id = 2, UserName = "B"}, 100},
                        {new UserDTO{Id = 3, UserName = "C"}, 50}
                    }, 
                    new List<ISearchAble>(){
                        new UserDTO{Id = 2, UserName = "B"},
                        new UserDTO{Id = 3, UserName = "C"},
                        new UserDTO{Id = 1, UserName = "A"}
                    }
                },

                new object[] { 
                    new Dictionary<ISearchAble, int>(){
                        {new UserDTO{Id = 1, UserName = "A"}, 10},
                        {new UserDTO{Id = 2, UserName = "B"}, 100},
                        {new UserDTO{Id = 3, UserName = "C"}, 1000}
                    }, 
                    new List<ISearchAble>(){
                        new UserDTO{Id = 3, UserName = "C"},
                        new UserDTO{Id = 2, UserName = "B"},
                        new UserDTO{Id = 1, UserName = "A"}
                    }
                }
            };

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