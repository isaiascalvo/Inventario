using Data;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class DiseaseRepository : GenericRepository<Client>, IDiseaseRepository
    {
        public DiseaseRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
