using System.Text.RegularExpressions;

namespace IRG
{
    public static class StringExtensions
    {
        public static string WithSpaces(this string str)
        {
            return Regex.Replace(str, "([a-z])([A-Z])", "$1 $2");
        }
    }
}