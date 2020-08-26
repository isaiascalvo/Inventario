using Data;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class OwnFeesRepository : GenericRepository<OwnFees>, IOwnFeesRepository
    {
        public OwnFeesRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
