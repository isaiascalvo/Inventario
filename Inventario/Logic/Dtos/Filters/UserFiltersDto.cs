using Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Logic.Dtos
{
    public class UserFiltersDto
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

        public Expression<Func<User, bool>> GetExpresion()
        {
            return x =>
                (Username == null || x.Username.ToLower().Contains(Username.ToLower())) &&
                (Name == null || x.Name.ToLower().Contains(Name.ToLower())) &&
                (Lastname == null || x.Lastname.ToLower().Contains(Lastname.ToLower())) &&
                (Dni == null || x.Dni == Dni) &&
                (Mail == null || x.Mail != null && x.Mail.ToLower().Contains(Mail.ToLower())) &&
                (Phone == null || x.Phone.Contains(Phone)) &&
                (IsAdmin == null || x.IsAdmin == IsAdmin) &&
                (Active == null || x.Active == Active);
        }
    }
}
