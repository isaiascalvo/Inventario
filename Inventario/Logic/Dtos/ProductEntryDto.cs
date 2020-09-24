using Data;
using System;
using System.Collections.Generic;
using System.Text;
using Util.Enums;

namespace Logic.Dtos
{
    public class ProductEntryDto: Entity
    {
        public DateTime Date { get; set; }
        public bool IsEntry { get; set; }
        public Guid? VendorId { get; set; }
        public VendorDto? Vendor { get; set; }
        public decimal? Cost { get; set; }
        public ePaymentTypes? PaymentType { get; set; }
        public string Observations { get; set; }
        public List<ProductEntryLineDto> ProductEntryLines { get; set; }
    }
}
