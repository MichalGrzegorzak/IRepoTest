using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interview.Implemenation
{
    public class User : IStoreable
    {
        public IComparable Id { get; set; }
        public string Name { get; set; }
    }
}
