﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class ProductEntryForCreationViewModel
    {
        public DateTime Date { get; set; }
        public bool IsEntry { get; set; }
        public Guid? VendorId { get; set; }
        public List<ProductEntryLineForCreationViewModel> ProductEntryLines { get; set; }
    }
}
