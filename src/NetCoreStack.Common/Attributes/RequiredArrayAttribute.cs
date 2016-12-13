using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace NetCoreStack.Common
{
    public class RequiredArrayAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count > 0;
            }
            return false;
        }
    }
}
