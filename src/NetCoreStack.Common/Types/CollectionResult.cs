using System.Collections;
using System.Collections.Generic;

namespace NetCoreStack.Common
{
    public abstract class CollectionResultBase
    {
        public int Draw { get; set; }
        
        public long TotalRecords { get; set; }
        
        public long TotalRecordsFiltered { get; set; }
        
        public object Error { get; set; }
    }

    public class CollectionResult : CollectionResultBase
    {
        public IEnumerable Data { get; set; }
    }
    
    public class CollectionResult<T> : CollectionResultBase
    {
        public IEnumerable<T> Data { get; set; }
    }
}
