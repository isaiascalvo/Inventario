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
        public string? Phone { get; set; }
        public bool? Debtor { get; set; }

        public bool IsEmpty()
        {
            return Name == null && Lastname == null && Dni == null && Mail == null && Phone == null && Debtor == null;
        }

        public Expression<Func<Client, bool>> GetExpresion()
        {
            return x =>
                (Name == null || x.Name.ToLower().Contains(Name.ToLower())) &&
                (Lastname == null || x.Lastname.ToLower().Contains(Lastname.ToLower())) &&
                (Dni == null || x.Dni == Dni) &&
                (Mail == null || x.Mail != null && x.Mail.ToLower().Contains(Mail.ToLower())) &&
                (Phone == null || x.Phone.Contains(Phone)) &&
                (Debtor == null || x.Debtor == Debtor);
        }
    }
}
