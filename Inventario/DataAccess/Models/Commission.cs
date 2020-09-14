using System;
using System.Collections.Generic;
using System.Text;

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
        public double Price { get; set; }
        public string PaymentType { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }
}
