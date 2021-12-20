using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Search;
public static class SearchValidater
{
    //Can be expanded upon if more requirements are added.
    private static Regex regex = new Regex(@"^[0-9a-zA-ZæøåÆØÅ,\.\/\\\(\)\[\]\!\?\-\;\:\s]{0,127}$", RegexOptions.Compiled);
    
    public static bool ValidateSearchTermCharacters(string input) => regex.IsMatch(input);
}   