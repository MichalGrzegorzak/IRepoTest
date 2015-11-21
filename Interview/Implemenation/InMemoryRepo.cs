using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interview.Implemenation
{
    public class InMemoryRepo<T> : IRepository<T> where T : IStoreable
    {
        private List<T> entities;
        private DataContext context;

        public InMemoryRepo() : this(null)
        {
        }
        public InMemoryRepo(IList<T> entities)
        {
            //if (context == null)
            //    throw new ArgumentNullException("context");

            //context.Entities = context.Entities.OfType<T>().ToList();
            //if (entities == null)
            //    entities = new List<T>();
            this.entities = entities.ToList();
        }

        public IEnumerable<T> All()
        {
            return entities;
        }

        public void Delete(IComparable id)
        {
            var element = FindById(id);
            if (element != null)
                entities.Remove(element);
            else
            {
                //don`t like to throw exception here, but the contract here is hiding such info.
                throw new InvalidOperationException("Element was not found");
            }
        }

        public void Save(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            var idx = entities.FindIndex(x => x.Id.Equals(item.Id));
            if (idx < 0)
                entities.Add(item);
            else
                entities[idx] = item;
        }

        public T FindById(IComparable id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            return entities.SingleOrDefault(x => x.Id.CompareTo(id) == 0);
        }
    }
}
