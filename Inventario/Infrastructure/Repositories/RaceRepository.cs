using Data;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class RaceRepository : GenericRepository<Category>, ICategoryRepository
    {
        public RaceRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
