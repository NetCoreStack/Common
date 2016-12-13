using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace NetCoreStack.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ApiRouteAttribute : RouteAttribute
    {
        public Type Type { get; set; }

        public MediaTypeCollection ContentTypes { get; set; } = new MediaTypeCollection();

        public string RegionKey { get; set; }

        public ApiRouteAttribute(string template, Type type)
            : base(template)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            Type = type;

            ContentTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
        }
    }
}
