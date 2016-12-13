namespace NetCoreStack.Common
{
    public class BaseViewModel : IQueryableObject
    {
        [PropertyDescriptor(IsIdentity = true)]
        public long ID { get; set; }

        public ObjectState ObjectState { get; set; }

        public virtual bool IsNew
        {
            get
            {
                if (ID > 0)
                    return false;

                return true;
            }
        }
    }
}
