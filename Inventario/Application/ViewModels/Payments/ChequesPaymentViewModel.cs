using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ViewModels
{
    public class ChequesPaymentViewModel : PaymentViewModel
    {
        public List<ChequeViewModel> ListOfCheques { get; set; }
    }
}
