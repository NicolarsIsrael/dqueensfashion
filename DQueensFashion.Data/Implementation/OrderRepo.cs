using DQueensFashion.Core.Model;
using DQueensFashion.Data.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Data.Implementation
{
    public class OrderRepo : CoreRepo<Order>, IOrderRepo
    {
        public OrderRepo(DbContext ctx) : base(ctx)
        {

        }

        //other functions
        public IEnumerable<Order> GetAllOrdersWithRelationships()
        {
            return _dbContext.Set<Order>()
                .Include(order => order.LineItems.Select(lineItem=>lineItem.Product))
                .Where(order => !order.IsDeleted)
                .ToList();
        }

        public Order GetOrdertByIdWithRelationships(int orderId)
        {
            return _dbContext.Set<Order>()
                .Include(order => order.LineItems.Select(lineItem => lineItem.Product))
                .Where(order => order.Id == orderId)
                .Where(order => !order.IsDeleted)
                .FirstOrDefault();
        }
    }

}
