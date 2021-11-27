using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Search;

public enum SearchParam{User, Resource, Category}
public class SearchRequestForm
{
    public SearchParam searchParam {get; init;}
    
    [StringLength(127)]
    public string searchTerm { get; set; }


    public SearchRequestForm(SearchParam searchParam, string searchTerm){
        this.searchParam = searchParam;
        this.searchTerm = searchTerm;
    }

}