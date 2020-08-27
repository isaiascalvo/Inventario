using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ViewModels
{
    public class CreditCardViewModel: PaymentViewModel
    {
        public string CardType { get; set; }
        public string Bank { get; set; }
        public double Discount { get; set; }
    }
}
