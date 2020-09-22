using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class ProductEntryLineDto: Entity
    {
        public decimal Quantity { get; set; }
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; }
        public Guid ProductEntryId { get; set; }
        public ProductEntryDto ProductEntry { get; set; }
    }
}
