using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ViewModels
{
    public class CashViewModel: PaymentViewModel
    {
        public decimal Discount { get; set; }
    }
}
