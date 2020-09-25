using Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Util.Enums;

namespace Logic.Dtos
{
    public class FeesReportsFiltersDto
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public bool IsEmpty()
        {
            return DateFrom == null && DateTo == null;
        }

        public Expression<Func<Sale, bool>> GetExpresion()
        {
            return x =>
                (DateFrom == null || x.Date >= DateFrom) &&
                (DateTo == null || x.Date <= DateTo);
        }
    }
}
