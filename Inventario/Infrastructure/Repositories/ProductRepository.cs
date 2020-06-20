using Data;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
