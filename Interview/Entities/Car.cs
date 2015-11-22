using System;

namespace Interview.Implemenation
{
    public class Car : IStoreable
    {
        public IComparable Id { get; set; }
        public int ProductionYear { get; set; }
    }
}