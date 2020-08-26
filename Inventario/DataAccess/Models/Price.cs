using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Util.Enums;

namespace Data
{
    public class Price: Entity
    {
        [Required]
        public double Value { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        public ePriceTypes PriceType { get; set; }
    }
}
