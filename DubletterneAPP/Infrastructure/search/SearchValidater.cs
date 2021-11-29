using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Search{
    public static class SearchValidater
    {
        //If no more methods is added. This thould be moved into the SearchRequestForm
        private static Regex regex = new Regex(@"^[a-zA-ZæøåÆØÅ,\.\/\\\(\)\[\]\!\?\-\;\:\s]{0,127}$", RegexOptions.Compiled);
        
        public static bool ValidateSearchTermCharacters(string streng){
            return regex.IsMatch(streng);
        }


    }   
}