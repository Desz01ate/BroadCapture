using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BroadCapture.Helpers
{
    public static class StringHelpers
    {
        public static string ExtractCreateBy(string message)
        {
            var group = Regex.Match(message, @"(- \[.*?\])", RegexOptions.Compiled);
            var writer = Regex.Replace(group.Value.TrimStart('-', ' '), @"[\[\]]", "", RegexOptions.Compiled);
            return writer;
        }
    }
}
