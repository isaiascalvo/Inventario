using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class PriceForCreationDto
    {
        public double Value { get; set; }
        public Guid ProductId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
