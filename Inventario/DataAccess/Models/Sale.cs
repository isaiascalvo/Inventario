using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Util.Enums;

namespace Data
{
    public class Sale: Entity
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid? ClientId { get; set; }
        public Client Client { get; set; }
        public string ClientName { get; set; }
        public DateTime Date { get; set; }
        public double Quantity { get; set; }
        public double Amount { get { return Payment.GetTotal(); } }
        public ePaymentTypes PaymentType { get; set; }
        public Guid PaymentId { get; set; }
        public Payment Payment { get; set; }
    }
}
