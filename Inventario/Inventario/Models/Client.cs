using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data
{
    public class Client: Entity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Dni { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        [Required]
        public bool Active { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
