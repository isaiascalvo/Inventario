using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class DebitCard: Payment
    {
        public string CardType { get; set; }
        public string Bank { get; set; }
        public double Surcharge { get; set; }

        public DebitCard()
        {

        }
        public DebitCard(double amount, double surcharge)
        {
            Surcharge = surcharge;
            Amount = amount * (1 + surcharge / 100);
        }
    }
}
