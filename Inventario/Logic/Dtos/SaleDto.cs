using Data;
using System;
using System.Collections.Generic;
using System.Text;
using Util.Enums;

namespace Logic.Dtos
{
    public class SaleDto: Entity
    {
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; }
        public Guid? ClientId { get; set; }
        public ClientDto Client { get; set; }
        public string ClientName { get; set; }
        public DateTime Date { get; set; }
        public double Quantity { get; set; }
        //public double Amount { get; set; }
        public ePaymentTypes PaymentType { get; set; }
        public Guid PaymentId { get; set; }
        public OwnFeesDto? OwnFees { get; set; }
        public CashDto? Cash { get; set; }
        public CreditCardDto? CreditCard { get; set; }
        public DebitCardDto? DebitCard { get; set; }
        public ChequesPaymentDto? Cheques { get; set; }
    }
}
