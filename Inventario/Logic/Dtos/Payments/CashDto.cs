using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class CashDto: PaymentDto
    {
        public double Discount { get; set; }
    }
}
