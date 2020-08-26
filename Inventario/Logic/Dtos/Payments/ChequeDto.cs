using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class ChequeDto: PaymentDto
    {
        public string Nro { get; set; }
        public string Bank { get; set; }
    }
}
