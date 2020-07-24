using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class CategoryDto: Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
