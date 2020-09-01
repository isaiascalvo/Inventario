using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ViewModels
{
    public class ChequeViewModel
    {
        public Guid Id { get; set; }
        public Guid ChequesPaymentId { get; set; }
        public ChequesPaymentViewModel ChequesPayment { get; set; }
        public string Nro { get; set; }
        public string Bank { get; set; }
        public double Value { get; set; }
    }
}
