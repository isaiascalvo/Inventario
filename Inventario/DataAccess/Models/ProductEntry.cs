using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class ProductEntry : Entity
    {
        public DateTime Date { get; set; }
        public bool IsEntry { get; set; }
        public List<ProductEntryLine> ProductEntryLines { get; set; }
        public ProductEntry() : base()
        {
            ProductEntryLines = new List<ProductEntryLine>();
        }
    }
}
