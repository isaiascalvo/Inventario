using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class VendorForCreationDto
    {
        public string Name { get; set; }
        public string? CUIL { get; set; }
        public string Phone { get; set; }
        public string? Mail { get; set; }
        //public bool Active { get; set; }
        public string? Description { get; set; }
    }
}
