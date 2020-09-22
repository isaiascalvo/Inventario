using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class DetailViewModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public ProductViewModel Product { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
