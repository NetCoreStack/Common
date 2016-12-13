using System;

namespace NetCoreStack.Common.Extensions
{
    public static class StringExtensions
    {
        const string rawControllerDefinition = "[Controller]";
        const string regexForApi = "^I(.*)Api$";

        public static bool IsCaseInsensitiveEqual(this string instance, string comparing)
        {
            return string.Compare(instance, comparing, StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}
