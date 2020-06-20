using Data;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class AnimalRepository : GenericRepository<Product>, IAnimalRepository
    {
        public AnimalRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
