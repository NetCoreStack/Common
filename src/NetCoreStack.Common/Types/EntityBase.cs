using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreStack.Common
{
    public abstract class EntityBase : IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }
    }
}