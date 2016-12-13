using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetCoreStack.Common.Internal
{
    public class DefaultApplicationPartContainer : IApplicationPartContainer
    {
        protected ApplicationPartManager PartManager { get; }
        public HashSet<Assembly> AssemblyContainer { get; }

        public DefaultApplicationPartContainer(ApplicationPartManager partManager)
        {
            PartManager = partManager;
            AssemblyContainer = new HashSet<Assembly>();

            foreach (var item in PartManager.ApplicationParts.OfType<AssemblyPart>())
            {
                AssemblyContainer.Add(item.Assembly);
            }
        }
    }
}
