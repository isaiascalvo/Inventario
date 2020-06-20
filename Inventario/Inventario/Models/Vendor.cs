using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data
{
    public class Vendor: Entity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string CUIL { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        [Required]
        public bool Active { get; set; }
        public string Descripton { get; set; }
    }
}
