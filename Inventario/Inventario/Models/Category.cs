﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data
{
    public class Category: Entity
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
