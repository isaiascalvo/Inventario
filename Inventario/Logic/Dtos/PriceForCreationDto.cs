using System;
using System.Collections.Generic;
using System.Text;
using Util.Enums;

namespace Logic.Dtos
{
    public class PriceForCreationDto
    {
        public decimal Value { get; set; }
        public Guid ProductId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
