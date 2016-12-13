using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NetCoreStack.Common.Extensions
{
    public static class TypeCoreExtensions
    {
        public static readonly IDictionary<string, Type> PredefinedTypes = new Dictionary<string, Type>
        {
            [typeof(object).FullName] = typeof(object),
            [typeof(bool).FullName] = typeof(bool),
            [typeof(char).FullName] = typeof(char),
            [typeof(string).FullName] = typeof(string),
            [typeof(sbyte).FullName] = typeof(sbyte),
            [typeof(byte).FullName] = typeof(byte),
            [typeof(short).FullName] = typeof(short),
            [typeof(ushort).FullName] = typeof(ushort),
            [typeof(int).FullName] = typeof(int),
            [typeof(uint).FullName] = typeof(uint),
            [typeof(long).FullName] = typeof(long),
            [typeof(ulong).FullName] = typeof(ulong),
            [typeof(float).FullName] = typeof(float),
            [typeof(double).FullName] = typeof(double),
            [typeof(decimal).FullName] = typeof(decimal),
            [typeof(DateTime).FullName] = typeof(DateTime),
            [typeof(TimeSpan).FullName] = typeof(TimeSpan),
            [typeof(Guid).FullName] = typeof(Guid),
            [typeof(Math).FullName] = typeof(Math),
            [typeof(Convert).FullName] = typeof(Convert)
        };

        public static bool IsValueType(this Type type)
        {
            return IntrospectionExtensions.GetTypeInfo(type).IsValueType;
        }

        public static bool IsReferenceType(this Type type)
        {
            return !IntrospectionExtensions.GetTypeInfo(type).IsValueType && type != typeof(string);
        }

        public static bool IsGenericType(this Type type)
        {
            return IntrospectionExtensions.GetTypeInfo(type).IsGenericType;
        }

        public static Type GetEnumerableType(this Type type)
        {
            foreach (Type intType in type.GetInterfaces())
            {
                if (intType.IsGenericType() && intType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    return intType.GetGenericArguments()[0];
                }
            }
            return null;
        }

        public static bool IsGenericType(this PropertyInfo propInfo)
        {
            return IntrospectionExtensions.GetTypeInfo(propInfo.PropertyType).IsGenericType;
        }

        public static bool IsEnumType(this Type type)
        {
            return IntrospectionExtensions.GetTypeInfo(type.GetNonNullableType()).IsEnum;
        }

        public static bool IsNullableType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static bool IsNumericType(this Type type)
        {
            return type.GetNumericTypeKind() != 0;
        }

        public static int GetNumericTypeKind(this Type type)
        {
            if (type == null)
            {
                return 0;
            }
            type = type.GetNonNullableType();
            if (type.IsEnumType())
            {
                return 0;
            }
            if (type == typeof(char) || type == typeof(float) || type == typeof(double) || type == typeof(decimal))
            {
                return 1;
            }
            if (type == typeof(sbyte) || type == typeof(short) || type == typeof(int) || type == typeof(long))
            {
                return 2;
            }
            if (type == typeof(byte) || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong))
            {
                return 3;
            }
            return 0;
        }

        public static object DefaultValue(this Type type)
        {
            if (type.IsValueType())
                return Activator.CreateInstance(type);

            return null;
        }

        public static Type GetNonNullableType(this Type type)
        {
            if (!type.IsNullableType())
                return type;

            return type.GetGenericArguments()[0];
        }

        public static bool IsCompatibleWith(this Type source, Type target)
        {
            if (source == target)
                return true;

            if (!target.IsValueType())
            {
                return source.IsAssignableFrom(target);
            }

            Type nonNullableType = source.GetNonNullableType();
            Type nonNullableType2 = target.GetNonNullableType();
            if (nonNullableType != source && nonNullableType2 == target)
            {
                return false;
            }
            if (nonNullableType.IsEnumType() || nonNullableType2.IsEnumType())
            {
                return nonNullableType == nonNullableType2;
            }
            if (nonNullableType == typeof(sbyte))
            {
                return nonNullableType2 == typeof(sbyte) || nonNullableType2 == typeof(short) || nonNullableType2 == typeof(int) || nonNullableType2 == typeof(long) || nonNullableType2 == typeof(float) || nonNullableType2 == typeof(double) || nonNullableType2 == typeof(decimal);
            }
            if (nonNullableType == typeof(byte))
            {
                return nonNullableType2 == typeof(byte) || nonNullableType2 == typeof(short) || nonNullableType2 == typeof(ushort) || nonNullableType2 == typeof(int) || nonNullableType2 == typeof(uint) || nonNullableType2 == typeof(long) || nonNullableType2 == typeof(ulong) || nonNullableType2 == typeof(float) || nonNullableType2 == typeof(double) || nonNullableType2 == typeof(decimal);
            }
            if (nonNullableType == typeof(short))
            {
                return nonNullableType2 == typeof(short) || nonNullableType2 == typeof(int) || nonNullableType2 == typeof(long) || nonNullableType2 == typeof(float) || nonNullableType2 == typeof(double) || nonNullableType2 == typeof(decimal);
            }
            if (nonNullableType == typeof(ushort))
            {
                return nonNullableType2 == typeof(ushort) || nonNullableType2 == typeof(int) || nonNullableType2 == typeof(uint) || nonNullableType2 == typeof(long) || nonNullableType2 == typeof(ulong) || nonNullableType2 == typeof(float) || nonNullableType2 == typeof(double) || nonNullableType2 == typeof(decimal);
            }
            if (nonNullableType == typeof(int))
            {
                return nonNullableType2 == typeof(int) || nonNullableType2 == typeof(long) || nonNullableType2 == typeof(float) || nonNullableType2 == typeof(double) || nonNullableType2 == typeof(decimal);
            }
            if (nonNullableType == typeof(uint))
            {
                return nonNullableType2 == typeof(uint) || nonNullableType2 == typeof(long) || nonNullableType2 == typeof(ulong) || nonNullableType2 == typeof(float) || nonNullableType2 == typeof(double) || nonNullableType2 == typeof(decimal);
            }
            if (nonNullableType == typeof(long))
            {
                return nonNullableType2 == typeof(long) || nonNullableType2 == typeof(float) || nonNullableType2 == typeof(double) || nonNullableType2 == typeof(decimal);
            }
            if (nonNullableType == typeof(ulong))
            {
                return nonNullableType2 == typeof(ulong) || nonNullableType2 == typeof(float) || nonNullableType2 == typeof(double) || nonNullableType2 == typeof(decimal);
            }
            return nonNullableType == typeof(float) && (nonNullableType2 == typeof(float) || nonNullableType2 == typeof(double));
        }

        public static ParameterExpression GetParameterExpression(this Type type)
        {
            return Expression.Parameter(type, "item");
        }

        public static object CreateGenericCollectionResult(this Type elementType)
        {
            return Activator.CreateInstance(typeof(CollectionResult<>).MakeGenericType(new Type[] { elementType }));
        }

        public static IList CreateElementTypeAsGenericList(this Type elementType)
        {
            return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { elementType }));
        }

        public static IEnumerable CreateElementTypeAsEnumerable(this Type elementType)
        {
            return (IEnumerable)Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { elementType }));
        }

        public static string GetDisplayValue(this PropertyInfo propInfo)
        {
            var attr = propInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (attr == null)
                return propInfo.Name.ToString();

            return (attr.Length > 0) ? attr[0].Name : propInfo.Name.ToString();
        }

        public static bool EnableFilter(this PropertyInfo propInfo)
        {
            var attr = propInfo.GetCustomAttribute(typeof(PropertyDescriptorAttribute));

            if (attr == null)
                return true;

            var attribute = attr as PropertyDescriptorAttribute;
            return attribute.EnableFilter;
        }

        public static string HasRequiredMessage(this PropertyInfo propInfo)
        {
            var attr = propInfo.GetCustomAttribute(typeof(RequiredAttribute));

            if (attr == null)
                return string.Empty;

            var requiredAttribute = attr as RequiredAttribute;
            return requiredAttribute.ErrorMessage;
        }

        public static bool IsRequired(this PropertyInfo propInfo)
        {
            var attr = propInfo.GetCustomAttribute(typeof(RequiredAttribute));

            if (attr == null)
                return false;

            return true;
        }

        public static string GetTypeName(this Type type)
        {
            Type nonNullableType = type.GetNonNullableType();
            string text = nonNullableType.Name;
            if (type != nonNullableType)
            {
                text += '?';
            }
            return text;
        }

        public static bool IsInterface(this Type type)
        {
            return IntrospectionExtensions.GetTypeInfo(type).IsInterface;
        }

        public static IEnumerable<Type> SelfAndBaseClasses(this Type type)
        {
            while (type != null)
            {
                yield return type;
                type = IntrospectionExtensions.GetTypeInfo(type).BaseType;
            }
            yield break;
        }

        public static IEnumerable<Type> SelfAndBaseTypes(this Type type)
        {
            if (type.IsInterface())
            {
                List<Type> list = new List<Type>();
                TypeHelper.AddInterface(list, type);
                return list;
            }
            return type.SelfAndBaseClasses();
        }

        public static MemberInfo FindPropertyOrField(this Type type, string memberName, bool staticAccess)
        {
            BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | (staticAccess ? BindingFlags.Static : BindingFlags.Instance);
            foreach (Type current in type.SelfAndBaseTypes())
            {
                IEnumerable<MemberInfo> memberList = current.GetProperties(bindingFlags);
                IEnumerable<FieldInfo> fields = current.GetFields(bindingFlags);

                var combinedList = memberList.Concat(fields);

                MemberInfo[] members = combinedList.Where(x => x.Name.IsCaseInsensitiveEqual(memberName)).ToArray();

                if (members.Length != 0)
                {
                    return members[0];
                }
            }
            return null;
        }


        public static MemberInfo FindPropertyOrField(this Type type, string memberName)
        {
            MemberInfo memberInfo = type.FindPropertyOrField(memberName, false);
            if (memberInfo == null)
            {
                memberInfo = type.FindPropertyOrField(memberName, true);
            }
            return memberInfo;
        }

        public static PropertyInfo GetIndexerPropertyInfo(this Type type, params Type[] indexerArguments)
        {
            return type.GetProperties().FirstOrDefault(x => AreArgumentsApplicable(indexerArguments, x.GetIndexParameters()));
        }

        public static bool IsFormFile(this Type type)
        {
            if (typeof(IFormFile).IsAssignableFrom(type) ||
                typeof(IEnumerable<IFormFile>).IsAssignableFrom(type))
            {
                return true;
            }

            return false;
        }

        private static bool AreArgumentsApplicable(IEnumerable<Type> arguments, IEnumerable<ParameterInfo> parameters)
        {
            List<Type> list = Enumerable.ToList<Type>(arguments);
            List<ParameterInfo> list2 = Enumerable.ToList<ParameterInfo>(parameters);
            if (list.Count() != list2.Count())
            {
                return false;
            }
            for (int i = 0; i < list.Count(); i++)
            {
                if (list2[i].ParameterType != list[i])
                {
                    return false;
                }
            }
            return true;
        }


        internal static bool IsPredefinedType(this Type type)
        {
            foreach (KeyValuePair<string, Type> entry in PredefinedTypes)
            {
                if (entry.Value == type)
                    return true;
            }
            return false;
        }

        public static string FirstSortableProperty(this Type type)
        {
            PropertyInfo propertyInfo = ((IEnumerable<PropertyInfo>)type.GetProperties())
                .Where<PropertyInfo>((Func<PropertyInfo, bool>)(property => property.PropertyType.IsPredefinedType())).FirstOrDefault<PropertyInfo>();

            if (propertyInfo == (PropertyInfo)null)
                throw new NotSupportedException("PropertyInfo required");

            return propertyInfo.Name;
        }

        public static Type BaseType(this Type type)
        {
            return type.GetTypeInfo().BaseType;
        }

        public static bool IsPrimitive(this Type type)
        {
            return type.GetTypeInfo().IsPrimitive;
        }

        public static bool IsPrimitiveArrayType(this Type type)
        {
            if (type.IsArray)
            {
                Type elementType = TypeHelper.GetElementType(type);
                return elementType.IsPrimitive() || elementType.Equals(typeof(string));
            }

            return false;
        }

        public static bool IsPrimitiveIEnumerableType(this Type type)
        {
            if (type.IsGenericType() &&
                type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var genericType = type.GetTypeInfo().GenericTypeArguments[0];
                return genericType != null && !genericType.IsReferenceType();
            }

            return false;
        }

        public static string CheckRequiredAttributeMessage(this PropertyInfo propInfo)
        {
            var requiredAttr = propInfo.GetCustomAttribute<RequiredAttribute>();
            if (requiredAttr != null)
                return requiredAttr.ErrorMessage;

            var numericRequiredAttr = propInfo.GetCustomAttribute<NumericRequiredAttribute>();
            if (numericRequiredAttr != null)
                return numericRequiredAttr.ErrorMessage;

            var arrayRequiredAttr = propInfo.GetCustomAttribute<RequiredArrayAttribute>();
            if (arrayRequiredAttr != null)
                return arrayRequiredAttr.ErrorMessage;

            return string.Empty;
        }
    }
}
