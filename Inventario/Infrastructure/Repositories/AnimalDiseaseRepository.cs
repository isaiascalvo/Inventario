using Data;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class AnimalDiseaseRepository : GenericRepository<Price>, IAnimalDiseaseRepository
    {
        public AnimalDiseaseRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
