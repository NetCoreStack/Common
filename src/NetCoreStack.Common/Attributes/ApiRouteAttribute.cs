using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using NetCoreStack.Common.Extensions;

namespace NetCoreStack.Common
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public sealed class ApiRouteAttribute : RouteAttribute
    {
        public string RegionKey { get; set; }

        public MediaTypeCollection ContentTypes { get; set; } = new MediaTypeCollection();

        public ApiRouteAttribute(string template, string regionKey)
            : base(template)
        {
            if (!regionKey.HasValue())
                throw new ArgumentNullException(nameof(regionKey));

            RegionKey = regionKey;
            ContentTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
        }
    }
}
