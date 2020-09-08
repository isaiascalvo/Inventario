using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class UserFiltersViewModel
    {
        public string? Username { get; set; }
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? Dni { get; set; }
        public string? Mail { get; set; }
        public string? Phone { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? Active { get; set; }

        public bool IsEmpty()
        {
            return Username == null && Name == null && Lastname == null && Dni == null && Mail == null && Phone == null && IsAdmin == null && Active == null;
        }
    }
}
