using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class CreditCard: Payment
    {
        public string CardType { get; set; }
        public string Bank { get; set; }
        public double Discount { get; set; }
    }
}
