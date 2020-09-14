using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class MiscellaneousExpensesFiltersViewModel
    {
        public string? Description { get; set; }
        public string? DateFrom { get; set; }
        public string? DateTo { get; set; }
        public double? Value { get; set; }
        public string? Destination { get; set; }
        public bool? IsFixed { get; set; }

        public bool IsEmpty()
        {
            return Description == null && DateFrom == null && DateTo == null && Value == null && Destination == null && IsFixed == null;
        }
    }
}
