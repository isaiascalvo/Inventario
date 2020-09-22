using Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class CashForCreationDto : PaymentForCreationDto
    {
        public decimal Discount { get; set; }
    }
}
