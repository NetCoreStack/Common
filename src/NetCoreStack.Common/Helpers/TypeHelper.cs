using Microsoft.Extensions.DependencyInjection;
using NetCoreStack.Common.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NetCoreStack.Common
{
    public static class TypeHelper
    {
        public static bool IsDependency(Type type)
        {
            return typeof(IDependency).IsAssignableFrom(type) ||
                    type.GetTypeInfo().GetCustomAttribute<ApiRouteAttribute>() != null;
        }

        public static bool IsEntity(Type type)
        {
            return typeof(EntityBase).IsAssignableFrom(type);
        }

        public static bool IsViewModel(Type type)
        {
            return typeof(BaseViewModel).IsAssignableFrom(type);
        }

        public static bool IsCompositeType(Type type)
        {
            return typeof(BaseCompositeType).IsAssignableFrom(type);
        }

        public static bool IsApi(Type type)
        {
            return typeof(IApiContract).IsAssignableFrom(type) &&
                    type.GetTypeInfo().GetCustomAttribute<ApiRouteAttribute>() != null;
        }

        public static bool IsTransient(Type type)
        {
            return typeof(ITransientDependency).IsAssignableFrom(type);
        }

        public static bool IsSingleton(Type type)
        {
            return typeof(ISingletonDependency).IsAssignableFrom(type);
        }

        public static bool IsApiDependency(Type type)
        {
            return typeof(IDependency).IsAssignableFrom(type) ||
                    type.GetTypeInfo().GetCustomAttribute<ApiRouteAttribute>() != null;
        }

        public static string GetMemberName<T, TValue>(Expression<Func<T, TValue>> memberAccess)
        {
            return ((MemberExpression)memberAccess.Body).Member.Name;
        }

        public static void AddInterface(List<Type> types, Type type)
        {
            if (!types.Contains(type))
            {
                types.Add(type);
                Type[] interfaces = type.GetInterfaces();
                for (int i = 0; i < interfaces.Length; i++)
                {
                    Type type2 = interfaces[i];
                    AddInterface(types, type2);
                }
            }
        }

        public static IList<T> CreateElementTypeAsGenericList<T>()
        {
            return (IList<T>)Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { typeof(T) }));
        }

        public static IEnumerable<T> CreateElementTypeAsEnumerable<T>()
        {
            return (IEnumerable<T>)Activator.CreateInstance(typeof(IEnumerable<>).MakeGenericType(new Type[] { typeof(T) }));
        }


        public static Type GetElementType(Type enumerableType)
        {
            return GetElementType(enumerableType, null);
        }

        public static Type GetElementType(Type enumerableType, IEnumerable enumerable)
        {
            if (enumerableType.HasElementType)
            {
                return enumerableType.GetElementType();
            }

            if (enumerableType.IsGenericType() &&
                enumerableType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return enumerableType.GetTypeInfo().GenericTypeArguments[0];
            }

            Type ienumerableType = GetIEnumerableType(enumerableType);
            if (ienumerableType != null)
            {
                return ienumerableType.GetTypeInfo().GenericTypeArguments[0];
            }

            if (typeof(IEnumerable).IsAssignableFrom(enumerableType))
            {
                var first = enumerable?.Cast<object>().FirstOrDefault();

                return first?.GetType() ?? typeof(object);
            }

            throw new ArgumentException($"Unable to find the element type for type '{enumerableType}'.", nameof(enumerableType));
        }


        public static void Register(IServiceCollection services, Type serviceType, Type implementationType)
        {
            if (IsDependency(serviceType))
            {
                services.AddScoped(serviceType, implementationType);
            }
            else if (IsTransient(serviceType))
            {
                services.AddTransient(serviceType, implementationType);
            }
            else if (IsSingleton(serviceType))
            {
                services.AddSingleton(serviceType, implementationType);
            }
        }

        private static Type GetIEnumerableType(Type enumerableType)
        {
            try
            {
                return enumerableType.GetTypeInfo().ImplementedInterfaces.FirstOrDefault(t => t.Name == "IEnumerable`1");
            }
            catch (AmbiguousMatchException)
            {
                if (enumerableType.BaseType() != typeof(object))
                    return GetIEnumerableType(enumerableType.BaseType());

                return null;
            }
        }
    }
}
