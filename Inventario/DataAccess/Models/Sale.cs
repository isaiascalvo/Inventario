using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Util.Enums;

namespace Data
{
    public class Sale: Entity
    {
        public Guid? ClientId { get; set; }
        public Client Client { get; set; }
        public string ClientName { get; set; }
        public DateTime Date { get; set; }
        public ePaymentTypes PaymentType { get; set; }
        public Guid PaymentId { get; set; }
        public Payment Payment { get; set; }
        public List<Detail> Details { get; set; }
        public Sale()
        {
            Details = new List<Detail>();
        }
    }
}
