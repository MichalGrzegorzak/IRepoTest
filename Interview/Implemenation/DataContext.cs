using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interview.Implemenation
{
    public class DataContext : IDataContext
    {
        public List<IStoreable> Entities { get; set; }

        public DataContext()
        {}

        public DataContext(List<IStoreable> entities)
        {
            Entities = entities;
        }
    }

}
