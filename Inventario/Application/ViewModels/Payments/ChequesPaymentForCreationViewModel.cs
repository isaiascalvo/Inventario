using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ViewModels
{
    public class ChequesPaymentForCreationViewModel : PaymentForCreationViewModel
    {
        public List<ChequeForCreationViewModel> ListOfCheques { get; set; }
    }
}
