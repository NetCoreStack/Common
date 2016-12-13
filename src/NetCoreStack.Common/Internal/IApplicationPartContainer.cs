using System.Collections.Generic;
using System.Reflection;

namespace NetCoreStack.Common.Internal
{
    public interface IApplicationPartContainer
    {
        HashSet<Assembly> AssemblyContainer { get; }
    }
}
