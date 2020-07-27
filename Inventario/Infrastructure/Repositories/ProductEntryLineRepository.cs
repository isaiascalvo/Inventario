using Data;
using Infrastructure.EFCore;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class ProductEntryLineRepository : GenericRepository<ProductEntryLine>, IProductEntryLineRepository
    {
        public ProductEntryLineRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
