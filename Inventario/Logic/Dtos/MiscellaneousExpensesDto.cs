using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class MiscellaneousExpensesDto: Entity
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public string? Destination { get; set; }
        public bool IsFixed { get; set; }
    }
}