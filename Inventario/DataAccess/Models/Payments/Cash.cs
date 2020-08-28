using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class Cash: Payment
    {
        public double Discount { get; set; }
        public Cash(double amount, double discount)
        {
            Amount = amount * (1 - Discount / 100);
            Discount = discount;
        }
    }
}
