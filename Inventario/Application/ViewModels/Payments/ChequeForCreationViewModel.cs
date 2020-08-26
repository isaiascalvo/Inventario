using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ViewModels
{
    public class ChequeForCreationViewModel: PaymentForCreationViewModel
    {
        public string Nro { get; set; }
        public string Bank { get; set; }
    }
}
