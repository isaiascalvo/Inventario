using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ViewModels
{
    public class DebitCardForCreationViewModel : PaymentForCreationViewModel
    {
        public string CardType { get; set; }
        public string Bank { get; set; }
        public double Discount { get; set; }
        public double Surcharge { get; set; }
    }
}
