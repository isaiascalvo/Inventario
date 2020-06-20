using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data
{
    public class Price: Entity
    {
        [Required]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public DateTime DateTime { get; set; }

    }
}
