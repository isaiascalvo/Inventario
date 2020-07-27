using Data;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class ProductEntryRepository : GenericRepository<ProductEntry>, IProductEntryRepository
    {
        public ProductEntryRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
