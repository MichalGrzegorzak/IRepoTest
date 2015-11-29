using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interview.Implemenation
{
    public class DataContext : IDataContext
    {
        public Dictionary<IComparable, IStoreable> Entities { get; set; }

        public DataContext()
        {}

        public DataContext(Dictionary<IComparable, IStoreable> entities)
        {
            Entities = entities;
        }
        public DataContext(List<IStoreable> entities)
        {
            Entities = new Dictionary<IComparable, IStoreable>();
            foreach (IStoreable entity in entities)
            {
                Entities.Add(entity.Id, entity);
            }
        }
    }

}
