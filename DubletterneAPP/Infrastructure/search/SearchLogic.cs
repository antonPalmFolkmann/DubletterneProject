using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Search{

    public enum SearchParameter{Category, User, Resource}
    public class SearchLogic
    {
        public record MatchResourceWithScore{
            public string Title {get; set;}
            public int mathcingPoints {get; set;}
        }
        //im a god!!!!!
        public MatchResourceWithScore[] matches;
        public string[] SearchTerm;
        public SearchParameter SearchParam;

/*
        public SearchMAIN(string searchTerm, searchParameter SearchParameter){
            if !SearchValidater.ValidateSearchTermCharacters(searchTerm){

            }

            SearchTerm = splitStringAtSpace(searchTerm)
            SearchParameter = searchParameter

            switch SearchParameter
                case Users
                    f
                case Catagory
                    f
                case Resourse
                    searchResourse()
        }

        splitStringAtSpace() returns (Array af strings)

        searchUser(){

        }

        searchCatagory(){

        }

        searchResourse(){
            for s string : SearchTerm
            {
                queryDatabaseResourseContainingX
            }
        }

        queryDatabaseTitlesContainingX () return (Array af titler){
            call databaseManagers API 
        }

        queryDatabaseCatagoryContainingX () return (Array af titler){
            call databaseManagers API 
        }

        queryDatabaseUsersContainingX () return (Array af titler){
            call databaseManagers API 
        }
        */
    }
}