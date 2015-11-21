using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interview.Implemenation
{
    public interface IDataContext
    {
        IEnumerable<IStoreable> Entities { get; set; }
    }

    public class DataContext : IDataContext
    {
        public IEnumerable<IStoreable> Entities { get; set; }
    }

}
