﻿using System;
using System.Collections.Generic;
using System.Text;
using Util.Enums;

namespace Logic.Dtos
{
    public class ProductForCreationDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? VendorId { get; set; }
        public string Brand { get; set; }
        public bool Active { get; set; }
        public bool Available { get; set; }
    }
}
