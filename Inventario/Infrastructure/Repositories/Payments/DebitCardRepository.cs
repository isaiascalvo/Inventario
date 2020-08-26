using Data;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class DebitCardRepository : GenericRepository<DebitCard>, IDebitCardRepository
    {
        public DebitCardRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
