using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class ProductEntryForCreationDto
    {
        public DateTime Date { get; set; }
        public List<ProductEntryLineForCreationDto> ProductEntryLines { get; set; }
    }
}
