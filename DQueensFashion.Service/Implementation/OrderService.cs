using DQueensFashion.Core.Model;
using DQueensFashion.Data.Contract;
using DQueensFashion.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Implementation
{
    public class OrderService : IOrderService
    {
        IUnitOfWork uow;
        public OrderService(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public int GetOrderCount()
        {
            return uow.OrderRepo.Count();
        }

        public void CreateOrder(Order order)
        {
            if (!ValidateOrderDetails(order))
                throw new Exception();

            uow.OrderRepo.Add(order);
            uow.Save();
        }
        
        public IEnumerable<Order> GetAllOrders()
        {
            return uow.OrderRepo.GetAllOrdersWithRelationships();
        }


        private bool ValidateOrderDetails(Order order)
        {
            if (order == null)
                return false;

            if (order.LineItems.Count < 1)
                return false;

            if (order.TotalQuantity < 1 || order.TotalAmount < 0)
                return false;

            return true;
        }
    }
}