using System;

namespace NetCoreStack.Common
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class ApiTimeoutAttribute : Attribute
    {
        public TimeSpan Timeout { get; private set; }

        /// <summary>
        /// Timeout seconds
        /// </summary>
        /// <param name="seconds"></param>
        public ApiTimeoutAttribute(int seconds)
        {
            Timeout = TimeSpan.FromSeconds(seconds);
        }
    }
}
