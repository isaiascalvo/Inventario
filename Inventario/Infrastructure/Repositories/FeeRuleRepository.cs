using Data;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class FeeRuleRepository : GenericRepository<FeeRule>, IFeeRuleRepository
    {
        public FeeRuleRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
