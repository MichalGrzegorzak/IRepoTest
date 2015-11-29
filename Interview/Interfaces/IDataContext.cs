using System;
using System.Collections.Generic;

namespace Interview.Implemenation
{
    public interface IDataContext
    {
        Dictionary<IComparable, IStoreable> Entities { get; set; }
    }
}