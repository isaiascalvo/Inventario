using System;
using System.Collections.Generic;
using System.Text;
using Util.Enums;

namespace Data
{
    public class Commission: Entity
    {
        public Guid VendorId { get; set; }
        public Vendor Vendor { get; set; }
        public Guid? ClientId { get; set; }
        public Client Client { get; set; }
        public string ClientName { get; set; }
        public string  Product { get; set; }
        public decimal Price { get; set; }
        public ePaymentTypes PaymentType { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }
}
