using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public abstract class PaymentDto: Entity
    {
        public Guid SaleId { get; set; }
        public decimal Amount { get; set; }
    }
}
