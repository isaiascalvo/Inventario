using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ViewModels
{
    public abstract class PaymentViewModel: Entity
    {
        public Guid Id { get; set; }
        public Guid SaleId { get; set; }
        public double Amount { get; set; }
        public virtual double GetTotal() 
        {
            return Amount;
        }
    }
}
