using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class DebitCard: Payment
    {
        public string CardType { get; set; }
        public string Bank { get; set; }
        public decimal Discount { get; set; }
        public decimal Surcharge { get; set; }

        public DebitCard()
        {

        }
        public DebitCard(decimal amount, decimal discount, decimal surcharge)
        {
            Discount = discount;
            Surcharge = surcharge;
            Amount = Math.Ceiling(amount * 100 * (1 + (surcharge - discount) / 100)) / 100;
        }
    }
}
