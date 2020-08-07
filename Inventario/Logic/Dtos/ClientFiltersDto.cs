using Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Logic.Dtos
{
    public class ClientFiltersDto
    {
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? Dni { get; set; }
        public string? Mail { get; set; }

        public bool IsEmpty()
        {
            return Name == null && Lastname == null && Dni == null && Mail == null;
        }

        public Expression<Func<Client, bool>> GetExpresion()
        {
            return x =>
                (Name == null || x.Name.Contains(Name)) &&
                (Lastname == null || x.Lastname.Contains(Lastname)) &&
                (Dni == null || x.Dni == Dni) &&
                (Mail == null || x.Mail.Contains(Mail));
        }
    }
}
