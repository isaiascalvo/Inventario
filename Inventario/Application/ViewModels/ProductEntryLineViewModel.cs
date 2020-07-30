using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class ProductEntryLineViewModel
    {
        public Guid Id { get; set; }
        public double Quantity { get; set; }
        public Guid ProductId { get; set; }
        public ProductViewModel Product { get; set; }
        public Guid ProductEntryId { get; set; }
        public ProductEntryViewModel ProductEntry { get; set; }
    }
}
