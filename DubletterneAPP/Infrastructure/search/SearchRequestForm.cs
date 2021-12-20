using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Search;

public enum SearchParam{User, Resource, Exeption}
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
            this.searchParam = SearchParam.Exeption;
            Console.WriteLine("Failed to parse string to enum type SearchParam", e);
        }
        this.searchTerm = searchTerm;
    }
}