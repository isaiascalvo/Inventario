using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Util.Enums;

namespace Application.ViewModels
{
    public class SaleViewModel
    {
        public Guid Id { get; set; }
        public Guid? ClientId { get; set; }
        public ClientViewModel Client { get; set; }
        public string ClientName { get; set; }
        public DateTime Date { get; set; }
        public ePaymentTypes PaymentType { get; set; }
        public Guid PaymentId { get; set; }
        public OwnFeesViewModel? OwnFees { get; set; }
        public CashViewModel? Cash { get; set; }
        public CreditCardViewModel? CreditCard { get; set; }
        public DebitCardViewModel? DebitCard { get; set; }
        public ChequesPaymentViewModel? Cheques { get; set; }
        public List<DetailViewModel> Details { get; set; }
    }
}
