using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ViewModels
{
    public abstract class PaymentForCreationViewModel
    {
        public Guid SaleId { get; set; }
        public double Amount { get; set; }
    }
}
