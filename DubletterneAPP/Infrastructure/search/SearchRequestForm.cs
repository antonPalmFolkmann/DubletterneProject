using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Search;

public enum SearchParam{User, Resource, Exception}
public class SearchRequestForm
{
    public SearchParam? searchParam {get; init;}
    
    [StringLength(127)]
    public string searchTerm { get; set; }


    public SearchRequestForm(string searchParam, string searchTerm)
    {
        try {
            this.searchParam = (SearchParam) Enum.Parse(typeof(SearchParam), searchParam);
        }
        catch (ArgumentException e){
            //This will most likely never happen since all calls to the constructor are in some way hard coded. 
            this.searchParam = SearchParam.Exception;
            Console.WriteLine("Failed to parse string to enum type SearchParam", e);
        }
        this.searchTerm = searchTerm;
    }
}