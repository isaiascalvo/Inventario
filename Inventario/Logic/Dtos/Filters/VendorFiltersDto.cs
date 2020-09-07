using Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Logic.Dtos
{
    public class VendorFiltersDto
    {
        public string? Name { get; set; }
        public string? CUIL { get; set; }
        public string? Phone { get; set; }
        public string? Mail { get; set; }
        public string? Description { get; set; }

        public bool IsEmpty()
        {
            return Name == null && CUIL == null && Phone == null && Mail == null && Description == null;
        }

        public Expression<Func<Vendor, bool>> GetExpresion()
        {
            return x =>
                (Name == null || x.Name.ToLower().Contains(Name.ToLower())) &&
                (CUIL == null || x.CUIL != null && x.CUIL == CUIL) &&
                (Phone == null || x.Phone.Contains(Phone)) &&
                (Mail == null || x.Mail != null && x.Mail.ToLower().Contains(Mail.ToLower())) &&
                (Description == null || x.Description.ToLower().Contains(Description.ToLower()));
        }
    }
}
