using System;
using System.ComponentModel.DataAnnotations;

namespace NetCoreStack.Common
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NumericRequiredAttribute : ValidationAttribute
    {
        public string PropertyName { get; set; }

        public override bool IsValid(object value)
        {
            var flag = Convert.ToInt64(value) > 0;
            if (!flag)
                ErrorMessage = $"{PropertyName} için bir değer belirtmelisiniz";

            return flag;
        }
    }
}
