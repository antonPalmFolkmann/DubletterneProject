using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Search;

public enum SearchParam{User, Resource, Category}
public class SearchRequestForm
{
    public SearchParam searchParam {get; init;}
    
    [StringLength(127)]
    public string searchTerm { get; set; }


 public SearchRequestForm(string searchParamString, string searchTerm){

        try {
            this.searchParam = (SearchParam) Enum.Parse(typeof(SearchParam), searchParamString);
        }
        catch (ArgumentException e){
            Console.WriteLine("Fail to parse string to enum type SearchParam", e);
        }
        this.searchTerm = searchTerm;
    }
}