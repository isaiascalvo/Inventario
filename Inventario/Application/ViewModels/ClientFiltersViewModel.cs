using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class ClientFiltersViewModel
    {
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? Dni { get; set; }
        public string? Mail { get; set; }

        public bool IsEmpty()
        {
            return Name == null && Lastname == null && Dni == null && Mail == null;
        }
    }
}
