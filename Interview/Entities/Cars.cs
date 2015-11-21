using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interview.Implemenation
{
    public class Cars : IStoreable
    {
        public IComparable Id { get; set; }

        public int ProductionYear { get; set; }
    }
}
