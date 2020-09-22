using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class FeeRule: Entity
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public DateTime Date { get; set; }
        public int FeesAmountTo { get; set; }
        public decimal Percentage { get; set; }
    }
}
