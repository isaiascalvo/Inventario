using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class DebitCard: Payment
    {
        public string CardType { get; set; }
        public string Bank { get; set; }
        public double Discount { get; set; }
        public double Surcharge { get; set; }

        public DebitCard()
        {

        }
        public DebitCard(double amount, double discount, double surcharge)
        {
            Discount = discount;
            Surcharge = surcharge;
            Amount = amount * (1 + (surcharge - discount) / 100);
        }
    }
}
