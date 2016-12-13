using System;
using System.Collections.Generic;
using System.Reflection;

namespace NetCoreStack.Common
{
    public interface IMetadataTypeInfoProvider
    {
        IDictionary<string, Type> CandidateTypes { get; set; }
        
        IDictionary<Type, List<PropertyInfo>> DescriptorProperties { get; set; }

        IDictionary<string, List<EntityTypeDefinition>> NavigationProperties { get; set; }

        Type GetMetadata(string fullName);

        bool HasTypePropertyDescriptor(string fullName);

        List<PropertyInfo> GetPropertyDescriptorsOfType(string fullName);
    }
}
