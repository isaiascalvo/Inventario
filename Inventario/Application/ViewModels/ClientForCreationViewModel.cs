using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class ClientForCreationViewModel
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Dni { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public bool Active { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
