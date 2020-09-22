using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ViewModels
{
    public class CreditCardViewModel: PaymentViewModel
    {
        public string CardType { get; set; }
        public string Bank { get; set; }
        public decimal Discount { get; set; }
        public decimal Surcharge { get; set; }
    }
}
