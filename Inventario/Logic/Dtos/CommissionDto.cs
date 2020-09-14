using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class CommissionDto : Entity
    {
        public Guid VendorId { get; set; }
        public VendorDto Vendor { get; set; }
        public Guid? ClientId { get; set; }
        public ClientDto Client { get; set; }
        public string ClientName { get; set; }
        public string Product { get; set; }
        public double Price { get; set; }
        public string PaymentType { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }
}
