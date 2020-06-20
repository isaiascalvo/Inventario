using Data;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class PregnancyRepository : GenericRepository<Vendor>, IPregnancyRepository
    {
        public PregnancyRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
