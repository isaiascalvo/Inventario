using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class ChequeDto: Entity
    {
        public Guid ChequesPaymentId { get; set; }
        public ChequesPaymentDto ChequesPayment { get; set; }
        public string Nro { get; set; }
        public string Bank { get; set; }
        public decimal Value { get; set; }
    }
}
