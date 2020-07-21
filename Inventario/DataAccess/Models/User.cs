using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data
{
    public class User : Entity
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Dni { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        [Required]
        public bool Active { get; set; }
    }
}
