using System.Collections.Generic;
using System.Linq;
using Interview.Implemenation;

namespace Interview
{
    /// <summary>
    /// Derived from InMemoryRepo, to gain access to protected property - 'entities'
    /// Some might call it a 'hack', but I think it`s quite usefull & safe trick
    /// </summary>
    public class InMemoryRepoForTests<T> : InMemoryRepo<T> where T : IStoreable
    {
        public InMemoryRepoForTests(IDataContext context) : base(context)
        {
        }

        public List<T> Entities
        {
            get { return _context.Entities.OfType<T>().ToList(); }
        }
    }
}