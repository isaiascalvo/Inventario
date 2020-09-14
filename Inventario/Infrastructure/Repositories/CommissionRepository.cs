using Data;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class CommissionRepository : GenericRepository<Commission>, ICommissionRepository
    {
        public CommissionRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
