using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class Cheque: Entity
    {
        public Guid ChequesPaymentId { get; set; }
        public ChequesPayment ChequesPayment { get; set; }
        public string Nro { get; set; }
        public string Bank { get; set; }
        public double Value { get; set; }
    }
}
