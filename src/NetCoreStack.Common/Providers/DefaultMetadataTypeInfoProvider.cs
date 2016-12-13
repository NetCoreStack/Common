using NetCoreStack.Common.Extensions;
using NetCoreStack.Common.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetCoreStack.Common.Providers
{
    public class DefaultMetadataTypeInfoProvider
    {
        private readonly HashSet<Assembly> _lookupAssemblies;
        private Assembly CurrentAssembly { get; }
        public IDictionary<string, Type> CandidateTypes { get; set; }
        public IDictionary<Type, List<PropertyInfo>> DescriptorProperties { get; set; }
        public IDictionary<string, List<EntityTypeDefinition>> NavigationProperties { get; set; }

        private void InitializePropertyDescriptorTypes()
        {
            var typeInfos = CandidateTypes
                    .Select(x => x.Value).ToList();

            foreach (var item in typeInfos)
            {
                var typeProperties = item.GetProperties();
                foreach (var propInfo in typeProperties)
                {
                    var attr = propInfo.GetCustomAttribute<PropertyDescriptorAttribute>();
                    if (attr != null)
                    {
                        var attribute = attr as PropertyDescriptorAttribute;
                        if (attr.CompossibleItem != CompossibleItemTypes.Unset)
                        {
                            List<PropertyInfo> props = new List<PropertyInfo>();
                            if (DescriptorProperties.TryGetValue(item, out props))
                                props.Add(propInfo);
                            else
                                DescriptorProperties.Add(item, new List<PropertyInfo> { propInfo });
                        }
                    }
                }
            }
        }

        private void InitializeDbModelNavigationProperties()
        {
            var typeInfos = CandidateTypes.Where(x => TypeHelper.IsEntity(x.Value))
                    .Select(x => x.Value.GetTypeInfo()).ToList();

            var excludeTypeList = new List<string>
            {
                nameof(EntityBase),
                nameof(IObjectState),
            };

            foreach (var item in typeInfos)
            {
                if (excludeTypeList.Contains(item.Name))
                    continue;

                Type itemType = item.AsType();
                var checkGenericType = itemType.GetGenericArguments();
                if (checkGenericType != null && checkGenericType.Count() > 0)
                {
                    itemType = itemType.GetGenericArguments()[0];
                }

                var props = item.AsType().GetProperties();

                foreach (var prop in props)
                {
                    Type underlyingType = null;
                    string fullname = prop.PropertyType.FullName;
                    bool isCollection = false;
                    EntityTypeDefinition definition = new EntityTypeDefinition();
                    if (typeof(ICollection<IObjectState>).IsAssignableFrom(prop.PropertyType) ||
                        typeof(IEnumerable<IObjectState>).IsAssignableFrom(prop.PropertyType))
                    {
                        isCollection = true;
                        underlyingType = prop.PropertyType.GetGenericArguments()[0];
                        fullname = underlyingType.FullName;
                    }
                    if (underlyingType == null && TypeHelper.IsEntity(prop.PropertyType))
                    {
                        underlyingType = prop.PropertyType;
                    }

                    if (underlyingType != null)
                    {
                        definition.PropertyName = prop.Name;
                        definition.IsCollection = isCollection;
                        definition.Metadata = fullname;
                        List<EntityTypeDefinition> definitions = new List<EntityTypeDefinition>();
                        if (NavigationProperties.TryGetValue(itemType.FullName, out definitions))
                        {
                            definitions.Add(definition);
                        }
                        else
                        {
                            definitions = new List<EntityTypeDefinition> { definition };
                            NavigationProperties.Add(itemType.FullName, definitions);
                        }
                    }
                }
            }
        }

        public DefaultMetadataTypeInfoProvider(IApplicationPartContainer partContainer)
        {
            _lookupAssemblies = partContainer.AssemblyContainer;
            CurrentAssembly = typeof(DefaultMetadataTypeInfoProvider).GetTypeInfo().Assembly;
            CandidateTypes = new Dictionary<string, Type>();
            DescriptorProperties = new Dictionary<Type, List<PropertyInfo>>();
            NavigationProperties = new Dictionary<string, List<EntityTypeDefinition>>();
            ResolveTypes();
            InitializePropertyDescriptorTypes();
            InitializeDbModelNavigationProperties();
        }

        private void ResolveTypes()
        {
            if (_lookupAssemblies == null)
                throw new ArgumentNullException(nameof(_lookupAssemblies));

            var cachedTypeList = new List<Type>();
            foreach (var item in _lookupAssemblies)
            {
                cachedTypeList.AddRange(item.GetTypes()
                .Where(x => TypeHelper.IsCompositeType(x) || TypeHelper.IsViewModel(x)).ToList());
            }

            foreach (var type in cachedTypeList)
            {
                CandidateTypes.Add(type.FullName, type);
            }
        }

        public Type GetMetadata(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                return null;

            Type predefinedType = null;
            if (TypeCoreExtensions.PredefinedTypes.TryGetValue(fullName, out predefinedType))
                return predefinedType;

            Type cacheType = null;
            if (CandidateTypes.TryGetValue(fullName, out cacheType))
                return cacheType;

            Type resolvedType = CurrentAssembly.GetType(fullName);
            if (resolvedType != null)
                return resolvedType;

            throw new ArgumentOutOfRangeException($"Sepecified known type could not be found! {Environment.NewLine}");
        }

        public bool HasTypePropertyDescriptor(string fullName)
        {
            return DescriptorProperties.Where(x => x.Key.FullName == fullName).Count() > 0;
        }

        public List<PropertyInfo> GetPropertyDescriptorsOfType(string fullName)
        {
            List<PropertyInfo> items = DescriptorProperties.SingleOrDefault(x => x.Key.FullName == fullName).Value;
            if (items == null)
                return new List<PropertyInfo>();
            return items;
        }
    }
}
