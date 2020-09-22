using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class Cash: Payment
    {
        public decimal Discount { get; set; }
        public Cash()
        {
                
        }
        public Cash(decimal amount, decimal discount)
        {
            Discount = discount;
            Amount = Math.Ceiling(amount * 100 * (1 - Discount) / 100) / 100;
        }
    }
}
