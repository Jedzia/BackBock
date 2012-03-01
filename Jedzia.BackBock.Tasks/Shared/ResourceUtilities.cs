namespace Jedzia.BackBock.Tasks.Shared
{
    using System.Globalization;
    using System.Text.RegularExpressions;

    internal static class ResourceUtilities
    {
        // Fields
        private static readonly Regex msbuildMessageCodePattern = new Regex(@"^\s*(?<CODE>MSB\d\d\d\d):\s*(?<MESSAGE>.*)$", RegexOptions.Singleline);

        // Methods
        internal static string ExtractMessageCode(Regex messageCodePattern, string messageWithCode, out string code)
        {
            code = null;
            string str = messageWithCode;
            if (messageCodePattern == null)
            {
                messageCodePattern = msbuildMessageCodePattern;
            }
            Match match = messageCodePattern.Match(messageWithCode);
            if (match.Success)
            {
                code = match.Groups["CODE"].Value;
                str = match.Groups["MESSAGE"].Value;
            }
            return str;
        }

        internal static string FormatResourceString(string resourceName, params object[] args)
        {
            string str;
            string str2;
            return FormatResourceString(out str, out str2, resourceName, args);
        }

        internal static string FormatResourceString(out string code, out string helpKeyword, string resourceName, params object[] args)
        {
            helpKeyword = GetHelpKeyword(resourceName);
            return ExtractMessageCode(null, FormatString(AssemblyResources.GetString(resourceName), args), out code);
        }

        internal static string FormatString(string unformatted, params object[] args)
        {
            string str = unformatted;
            if ((args != null) && (args.Length > 0))
            {
                str = string.Format(CultureInfo.CurrentCulture, unformatted, args);
            }
            return str;
        }

        private static string GetHelpKeyword(string resourceName)
        {
            return ("MSBuild." + resourceName);
        }
    }
}