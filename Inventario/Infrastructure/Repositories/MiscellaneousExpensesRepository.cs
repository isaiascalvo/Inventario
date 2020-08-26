using Data;
using Data.Models;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class MiscellaneousExpensesRepository : GenericRepository<MiscellaneousExpenses>, IMiscellaneousExpensesRepository
    {
        public MiscellaneousExpensesRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
