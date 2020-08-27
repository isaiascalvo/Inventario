using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class MiscellaneousExpenses: Entity
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public string? Destination { get; set; }
    }
}