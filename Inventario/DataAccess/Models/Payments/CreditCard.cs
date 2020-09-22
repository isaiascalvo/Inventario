using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class CreditCard: Payment
    {
        public string CardType { get; set; }
        public string Bank { get; set; }
        public decimal Discount { get; set; }
        public decimal Surcharge { get; set; }

        public CreditCard()
        {

        }
        public CreditCard(decimal amount, decimal discount, decimal surcharge)
        {
            Discount = discount;
            Surcharge = surcharge;
            Amount = Math.Ceiling(amount * 100 * (1 + (surcharge - discount) / 100)) / 100;
        }
    }
}
