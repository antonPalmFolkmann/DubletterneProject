using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


//Inpiration taken from the guide: https://code-maze.com/blazor-webassembly-pagination/
namespace Infrastructure.Search;

public enum SearchParam{User, Resource, Category}
public class SearchRequestForm
{
    const int maxPageSize = 50; 
    public int PageNumber { get; set; } = 1; 
    private int _pageSize = 4; 
    public int PageSize 
    { 
        get 
        { 
            return _pageSize; 
        } 
        set 
        { 
            _pageSize = (value > maxPageSize) ? maxPageSize : value; 
        }
    }



    public SearchParam searchParam {get; init;}
    
    [StringLength(127)]
    public string searchTerm { get; set; }


/*
    public SearchRequestForm(SearchParam searchParam, string searchTerm){
        this.searchParam = searchParam;
        this.searchTerm = searchTerm;
    }
*/
}