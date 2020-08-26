using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public abstract class Payment: Entity
    {
        public Guid SaleId { get; set; }
        public double Amount { get; set; }
        public virtual double GetTotal() 
        {
            return Amount;
        }
    }
}
