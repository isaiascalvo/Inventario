using Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Logic.Dtos
{
    public class ProductEntryFiltersDto
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool? IsEntry { get; set; }
        public Guid? VendorId { get; set; }

        public bool IsEmpty()
        {
            return DateFrom == null && DateTo == null && IsEntry == null && VendorId == null;
        }

        public Expression<Func<ProductEntry, bool>> GetExpresion()
        {
            return x =>
                (IsEntry == null || x.IsEntry == IsEntry) &&
                (DateFrom == null || x.Date >= DateFrom) &&
                (DateTo == null || x.Date <= DateTo) &&
                (VendorId == null || x.VendorId != null && x.VendorId == VendorId);
        }
    }
}
