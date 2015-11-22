using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interview.Implemenation
{
    public class InMemoryRepo<T> : IRepository<T> where T : IStoreable
    {
        protected readonly IDataContext _context;

        public InMemoryRepo(IDataContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _context = context;
        }

        public IEnumerable<T> All()
        {
            return _context.Entities.OfType<T>();
        }

        public void Delete(IComparable id)
        {
            var element = FindById(id);
            if (element != null)
                _context.Entities.Remove(element);
            else
            {
                //don`t like it, but in given contract, that`s the only way
                throw new InvalidOperationException("Delete failed, id not found");
            }
        }

        public void Save(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            var entities = _context.Entities;
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

            return _context.Entities.OfType<T>().SingleOrDefault(x => x.Id.CompareTo(id) == 0);
        }
    }
}
