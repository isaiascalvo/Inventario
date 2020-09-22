using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class DetailDto: Entity
    {
        public Guid SaleId { get; set; }
        public SaleDto Sale { get; set; }
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
