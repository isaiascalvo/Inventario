using Data;
using Data.Models;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class ChequehRepository : GenericRepository<Cheque>, IChequeRepository
    {
        public ChequehRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
