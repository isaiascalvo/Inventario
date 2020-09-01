using System;
using System.Collections.Generic;
using System.Text;
using Util.Enums;

namespace Logic.Dtos
{
    public class SaleForCreationDto
    {
        public Guid ProductId { get; set; }
        public Guid? ClientId { get; set; }
        public string ClientName { get; set; }
        public DateTime Date { get; set; }
        public double Quantity { get; set; }
        public ePaymentTypes PaymentType { get; set; }
        public OwnFeesForCreationDto? OwnFees { get; set; }
        public CashForCreationDto? Cash { get; set; }
        public CreditCardForCreationDto? CreditCard { get; set; }
        public DebitCardForCreationDto? DebitCard { get; set; }
        public ChequesPaymentForCreationDto? Cheques { get; set; }
    }
}
