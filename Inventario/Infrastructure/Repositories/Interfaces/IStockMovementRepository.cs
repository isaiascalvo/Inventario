using Data.Models;
using Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IStockMovementRepository : IGenericRepository<StockMovement>
    {
    }
}
