using System;

namespace NetCoreStack.Common.Extensions
{
    internal static class StringExtensions
    {
        const string rawControllerDefinition = "[Controller]";
        const string regexForApi = "^I(.*)Api$";

        internal static bool IsCaseInsensitiveEqual(this string instance, string comparing)
        {
            return string.Compare(instance, comparing, StringComparison.OrdinalIgnoreCase) == 0;
        }

        internal static bool HasValue(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}
