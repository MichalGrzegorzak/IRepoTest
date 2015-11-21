using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interview.Implemenation
{
    public class InMemoryRepo<T> : IRepository<T> where T : IStoreable
    {
        private readonly IList<T> entities;
        private DataContext context;

        public InMemoryRepo() : this(null)
        {
        }
        public InMemoryRepo(DataContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            entities = context.Entities.OfType<T>().ToList();
            if (entities == null)
                entities = new List<T>();
            //this.entities = entities;
        }

        public IEnumerable<T> All()
        {
            return entities.OfType<T>();
        }

        public void Delete(IComparable id)
        {
            var element = FindById(id);
            if(element != null)
                entities.Remove(element);
        }

        public void Save(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            entities.Add(item);
        }

        public T FindById(IComparable id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            return entities.SingleOrDefault(x => x.Id.CompareTo(id) == 0);
        }
    }
}
