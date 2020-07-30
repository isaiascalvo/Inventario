using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class ProductEntryViewModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsEntry { get; set; }
        public List<ProductEntryLineViewModel> ProductEntryLines { get; set; }
    }
}
