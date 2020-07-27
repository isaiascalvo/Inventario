using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class ProductEntryDto: Entity
    {
        public DateTime Date { get; set; }
        public List<ProductEntryLineDto> ProductEntryLines { get; set; }
    }
}
