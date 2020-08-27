using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ViewModels
{
    public class DebitCardViewModel: PaymentViewModel
    {
        public string CardType { get; set; }
        public string Bank { get; set; }
        public double Surcharge { get; set; }
    }
}
