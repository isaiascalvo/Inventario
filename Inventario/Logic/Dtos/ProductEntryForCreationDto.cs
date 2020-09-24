using System;
using System.Collections.Generic;
using System.Text;
using Util.Enums;

namespace Logic.Dtos
{
    public class ProductEntryForCreationDto
    {
        public DateTime Date { get; set; }
        public bool IsEntry { get; set; }
        public Guid? VendorId { get; set; }
        public decimal? Cost { get; set; }
        public ePaymentTypes? PaymentType { get; set; }
        public string Observations { get; set; }
        public List<ProductEntryLineForCreationDto> ProductEntryLines { get; set; }
    }
}
