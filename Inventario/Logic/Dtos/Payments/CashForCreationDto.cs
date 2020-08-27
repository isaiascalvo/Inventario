using Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class CashForCreationDto : PaymentForCreationDto
    {
        public double Discount { get; set; }
    }
}
