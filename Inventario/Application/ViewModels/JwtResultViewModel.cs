using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class JwtResultViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Mail { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Token { get; set; }
        //public IEnumerable<string> Roles { get; set; }
        //public IEnumerable<string> CodigosMenues { get; set; }
    }
}
