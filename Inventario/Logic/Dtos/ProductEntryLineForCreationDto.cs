using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class ProductEntryLineForCreationDto
    {
        public double Quantity { get; set; }
        public Guid ProductId { get; set; }
        public Guid? ProductEntryId { get; set; }
    }
}
