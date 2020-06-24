using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class PriceViewModel
    {
        public Guid Id { get; set; }
        public double Value { get; set; }
        public Guid ProductId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
