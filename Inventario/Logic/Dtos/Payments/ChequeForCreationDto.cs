using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class ChequeForCreationDto: PaymentForCreationDto
    {
        public string Nro { get; set; }
        public string Bank { get; set; }
    }
}
