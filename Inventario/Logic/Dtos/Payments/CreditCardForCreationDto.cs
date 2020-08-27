using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class CreditCardForCreationDto : PaymentForCreationDto
    {
        public string CardType { get; set; }
        public string Bank { get; set; }
        public double Discount { get; set; }
    }
}
