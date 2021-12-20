using Infrastructure.Search;

namespace Infrastructure.Tests.Search;

public class MatchSorterTests : IDisposable
{
    [Theory]
    [MemberData(nameof(UserData))]
    public void sortMatchesHighestValueFirst_sorts_UserDTO_highest_first(Dictionary<UserDTO, int> keyValueDictonary, List<UserDTO> expected)
    {
        //Arrange

        //Act
        var actual = MatchSorter<UserDTO>.SortMatchesKeysHighestScoreFirst(keyValueDictonary);

        //Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ResourceData))]
    public void sortMatchesHighestValueFirst_sorts_ResourceDTO_highest_first(Dictionary<ResourceDTO, int> keyValueDictonary, List<ResourceDTO> expected)
    {
        //Arrange

        //Act
        var actual = MatchSorter<ResourceDTO>.SortMatchesKeysHighestScoreFirst(keyValueDictonary);

        //Assert
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> UserData =>
        new List<object[]>
        {
            new object[] {
                new Dictionary<UserDTO, int>(){
                    {new UserDTO{Id = 1, UserName = "A"}, 10},
                    {new UserDTO{Id = 2, UserName = "B"}, 100},
                    {new UserDTO{Id = 3, UserName = "C"}, 50}
                },
                new List<UserDTO>(){
                    new UserDTO{Id = 2, UserName = "B"},
                    new UserDTO{Id = 3, UserName = "C"},
                    new UserDTO{Id = 1, UserName = "A"}
                }
            }
        };

    public static IEnumerable<object[]> ResourceData =>
       new List<object[]>
           {
                new object[] {
                    new Dictionary<ResourceDTO, int>(){
                        {new ResourceDTO{Id = 1, Title = "A"}, 10},
                        {new ResourceDTO{Id = 2, Title = "B"}, 100},
                        {new ResourceDTO{Id = 3, Title = "C"}, 1000}
                },
                new List<ResourceDTO>(){
                    new ResourceDTO{Id = 3, Title = "C"},
                    new ResourceDTO{Id = 2, Title = "B"},
                    new ResourceDTO{Id = 1, Title = "A"}
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