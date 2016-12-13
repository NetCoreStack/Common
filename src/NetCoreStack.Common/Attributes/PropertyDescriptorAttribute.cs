using System;

namespace NetCoreStack.Common
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class PropertyDescriptorAttribute : Attribute
    {
        public string Name { get; set; }
        public string QueryStringName { get; set; }
        public string ComposeWith { get; set; }
        public string Composer { get; set; }
        public string ComparableWith { get; set; }
        public string ComplexComposer { get; set; }
        public bool EnableFilter { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsSelectable { get; set; }
        public string DataSourceUrl { get; set; }
        public string[] Args { get; set; }
        public string CascadeFrom { get; set; }
        public bool Sanitation { get; set; }
        public int Order { get; set; }
        public int MinimumInputLength { get; set; } = 2;
        public int CompossibleItem { get; set; }
        public FilterOperator DefaultFilterBehavior { get; set; } = FilterOperator.IsEqualTo;

        public PropertyDescriptorAttribute()
        {
            // Convention!
            IsIdentity = false;
            EnableFilter = true;
        }
    }
}
