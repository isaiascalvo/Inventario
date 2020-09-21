using Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Util.Enums;

namespace Logic.Dtos
{
    public class SaleFiltersDto
    {
        public string? ClientName { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? ProductId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public ePaymentTypes? PaymentType { get; set; }

        public bool IsEmpty()
        {
            return ClientName == null && ClientId == null && ProductId == null && DateFrom == null && DateTo == null && PaymentType == null;
        }

        public Expression<Func<Sale, bool>> GetExpresion()
        {
            return x =>
                (ClientId == null || x.ClientId == ClientId) &&
                (ClientName == null || x.ClientName.ToLower().Contains(ClientName.ToLower())) &&
                (ProductId == null || x.Details.Exists(x => x.ProductId == ProductId)) &&
                (PaymentType == null || x.PaymentType == PaymentType) &&
                (DateFrom == null || x.Date >= DateFrom) &&
                (DateTo == null || x.Date <= DateTo);
        }
    }
}
