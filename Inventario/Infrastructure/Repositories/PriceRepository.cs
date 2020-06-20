using Data;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class PriceRepository : GenericRepository<Price>, IPriceRepository
    {
        public PriceRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
