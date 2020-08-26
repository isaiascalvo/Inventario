using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class VendorForCreationViewModel
    {
        public string Name { get; set; }
        public string? CUIL { get; set; }
        public string Phone { get; set; }
        public string? Mail { get; set; }
        //public bool Active { get; set; }
        public string? Description { get; set; }
    }
}
