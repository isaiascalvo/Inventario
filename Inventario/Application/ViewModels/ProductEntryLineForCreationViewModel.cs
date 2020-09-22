using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class ProductEntryLineForCreationViewModel
    {
        public decimal Quantity { get; set; }
        public Guid ProductId { get; set; }
        public Guid? ProductEntryId { get; set; }
    }
}
