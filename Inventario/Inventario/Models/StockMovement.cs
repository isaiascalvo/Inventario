using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class StockMovement
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public DateTime DateTime { get; set; }
        public bool Entry { get; set; }
    }
}
