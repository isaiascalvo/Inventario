using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public abstract class PaymentForCreationDto
    {
        public Guid SaleId { get; set; }
        public double Amount { get; set; }
    }
}
