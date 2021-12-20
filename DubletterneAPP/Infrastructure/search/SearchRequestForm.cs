using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Search;

public enum SearchParam{User, Resource}
public class SearchRequestForm
{
    public SearchParam searchParam {get; init;}
    
    [StringLength(127)]
    public string searchTerm { get; set; }


 public SearchRequestForm(string searchParam, string searchTerm){

        try {
            this.searchParam = (SearchParam) Enum.Parse(typeof(SearchParam), searchParam);
        }
        catch (ArgumentException e){
            Console.WriteLine("Failed to parse string to enum type SearchParam", e);
        }
        this.searchTerm = searchTerm;
    }
}