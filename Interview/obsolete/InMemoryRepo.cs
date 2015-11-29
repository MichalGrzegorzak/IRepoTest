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
            return _context.Entities.Values.OfType<T>();
        }

        public void Delete(IComparable id)
        {
            var element = FindById(id);
            if (element != null)
                _context.Entities.Remove(element.Id);
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

            _context.Entities[item.Id] = item;
            //var idx = _context.Entities.FindIndex(x => x.Id.Equals(item.Id));
            //if (idx < 0)
            //    _context.Entities.Add(item);
            //else
            //    _context.Entities[idx] = item;
        }

        public T FindById(IComparable id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            if (_context.Entities.ContainsKey(id))
            {
                return _context.Entities.Values.OfType<T>().SingleOrDefault(x => x.Id.CompareTo(id) == 0);
            }
            return default(T);
        }
    }
}
