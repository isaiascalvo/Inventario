using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class ClientForCreationDto
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Dni { get; set; }
        public string Phone { get; set; }
        public string? Mail { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
