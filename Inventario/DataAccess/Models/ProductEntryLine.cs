using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class ProductEntryLine: Entity
    {
        public Guid ProductEntryId { get; set; }
        public ProductEntry ProductEntry { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public decimal Quantity { get; set; }
    }
}
