using Data;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class CashRepository : GenericRepository<Cash>, ICashRepository
    {
        public CashRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
