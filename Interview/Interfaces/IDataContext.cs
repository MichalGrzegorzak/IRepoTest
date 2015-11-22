using System.Collections.Generic;

namespace Interview.Implemenation
{
    public interface IDataContext
    {
        List<IStoreable> Entities { get; set; }
    }
}