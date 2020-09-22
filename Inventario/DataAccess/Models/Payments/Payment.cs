using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public abstract class Payment: Entity
    {
        public Guid SaleId { get; set; }
        public decimal Amount { get; set; }
    }
}
