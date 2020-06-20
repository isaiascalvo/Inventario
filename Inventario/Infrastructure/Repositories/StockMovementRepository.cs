using Data.Models;
using Infrastructure.EFCore;
using Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class StockMovementRepository : GenericRepository<StockMovement>, IStockMovementRepository
    {
        public StockMovementRepository(InventarioDbContext context) : base(context)
        {

        }
    }
}
