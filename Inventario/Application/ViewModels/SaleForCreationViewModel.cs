using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Util.Enums;

namespace Application.ViewModels
{
    public class SaleForCreationViewModel
    {
        public Guid? ClientId { get; set; }
        public string ClientName { get; set; }
        public DateTime Date { get; set; }
        public ePaymentTypes PaymentType { get; set; }
        public OwnFeesForCreationViewModel? OwnFees { get; set; }
        public CashForCreationViewModel? Cash { get; set; }
        public CreditCardForCreationViewModel? CreditCard { get; set; }
        public DebitCardForCreationViewModel? DebitCard { get; set; }
        public ChequesPaymentForCreationViewModel? Cheques { get; set; }
        public List<DetailForCreationViewModel> Details { get; set; }
    }
}
