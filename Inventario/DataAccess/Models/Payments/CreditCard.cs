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
        public double Surcharge { get; set; }

        public CreditCard()
        {

        }
        public CreditCard(double amount, double discount, double surcharge)
        {
            Discount = discount;
            Surcharge = surcharge;
            Amount = amount * (1 + (surcharge - discount) / 100);
        }
    }
}
