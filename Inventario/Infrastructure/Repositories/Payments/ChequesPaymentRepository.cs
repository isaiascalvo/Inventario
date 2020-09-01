using Data;
using Data.Models;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class ChequesPaymentRepository : GenericRepository<ChequesPayment>, IChequesPaymentRepository
    {
        public ChequesPaymentRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
