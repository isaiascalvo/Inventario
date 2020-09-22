using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ViewModels
{
    public abstract class PaymentViewModel
    {
        public Guid Id { get; set; }
        public Guid SaleId { get; set; }
        public decimal Amount { get; set; }
    }
}
