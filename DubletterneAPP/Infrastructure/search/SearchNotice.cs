using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Search{
    public enum SeachStatus{Successfull, Failed}
    public class SearchNotice
    {
        public SeachStatus seachStatus { get; set;}

        
    }
}