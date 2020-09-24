using iText.Kernel.XMP.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using Util.Enums;

namespace Data
{
    public class ProductEntry : Entity
    {
        public DateTime Date { get; set; }
        public bool IsEntry { get; set; }
        public Guid? VendorId { get; set; }
        public Vendor? Vendor { get; set; }
        public decimal? Cost { get; set; }
        public ePaymentTypes? PaymentType { get; set; }
        public string Observations { get; set; }
        public List<ProductEntryLine> ProductEntryLines { get; set; }
        public ProductEntry() : base()
        {
            ProductEntryLines = new List<ProductEntryLine>();
        }
    }
}
