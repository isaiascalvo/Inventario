using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class ChequeForCreationDto
    {
        public Guid ChequesPaymentId { get; set; }
        public string Nro { get; set; }
        public string Bank { get; set; }
        public double Value { get; set; }
    }
}
