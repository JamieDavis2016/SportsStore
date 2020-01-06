using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private ApplicationDbContext DbContext;

        public EFOrderRepository(ApplicationDbContext context) {
            DbContext = context;
        }

        public IQueryable<Order> Orders => DbContext.Orders
            .Include(x => x.Lines)
            .ThenInclude(x => x.Product);

        public void SaveOrder(Order order)
        {
            DbContext.AttachRange(order.Lines.Select(x => x.Product));
            if(order.OrderID == 0)
            {
                DbContext.Orders.Add(order);
            }
            DbContext.SaveChanges();
        }
    }
}
