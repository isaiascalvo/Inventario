using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ViewModels
{
    public class MiscellaneousExpensesForCreationViewModel
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public string? Destination { get; set; }
        public bool IsFixed { get; set; }
    }
}