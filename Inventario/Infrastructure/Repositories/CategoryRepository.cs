using Data;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
