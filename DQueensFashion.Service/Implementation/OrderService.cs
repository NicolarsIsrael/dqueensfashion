using DQueensFashion.Core.Model;
using DQueensFashion.Data.Contract;
using DQueensFashion.Service.Contract;
using DQueensFashion.Utilities;
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

            foreach(var lineItem in order.LineItems)
            {
                Product p = uow.ProductRepo.Get(lineItem.Product.Id);
                if (p.CategoryId != AppConstant.CustomMadeCategoryId || p.CategoryId != AppConstant.ReadyMadeCategoryId)
                {
                    p.Quantity -= lineItem.Quantity;
                    if (p.Quantity < 0)
                        p.Quantity = 0;
                }
            }

            uow.OrderRepo.Add(order);
            uow.Save();
        }

        public IEnumerable<Order> GetAllOrdersForCustomer(int customerId)
        {
            var orders = uow.OrderRepo.GetAllOrdersWithRelationships().Where(order=>order.CustomerId==customerId);
            return orders;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return uow.OrderRepo.GetAllOrdersWithRelationships();
        }

        public IEnumerable<Order> GetProcessingOrders()
        {
            return uow.OrderRepo.GetAllOrdersWithRelationships()
                .Where(order => order.OrderStatus == OrderStatus.Processing);
        }

        public IEnumerable<Order> GetInTransitOrders()
        {
            return uow.OrderRepo.GetAllOrdersWithRelationships()
                .Where(order => order.OrderStatus == OrderStatus.InTransit);
        }

        public IEnumerable<Order> GetDeliveredOrders()
        {
            return uow.OrderRepo.GetAllOrdersWithRelationships()
                .Where(order => order.OrderStatus == OrderStatus.Delivered);
        }

        public IEnumerable<Order> GetReturnedOrders()
        {
            return uow.OrderRepo.GetAllOrdersWithRelationships()
                .Where(order => order.OrderStatus == OrderStatus.Returned);
        }

        public IEnumerable<Order> GetDeletedOrders()
        {
            return uow.OrderRepo.GetAllOrdersWithRelationships()
                .Where(order => order.OrderStatus == OrderStatus.Deleted);
        }

        public IEnumerable<Order> GetCompletedOrders()
        {
            return uow.OrderRepo.GetAllOrdersWithRelationships()
    .Where(order => order.OrderStatus == OrderStatus.Completed);
        }

        public Order GetOrderById(int id)
        {
            return uow.OrderRepo.GetOrdertByIdWithRelationships(id);
        }

        public void UpdateOrder(Order order)
        {
            if (!ValidateOrderDetails(order))
                throw new Exception();

            uow.OrderRepo.Update(order);
            uow.Save();
        }

        private bool ValidateOrderDetails(Order order)
        {
            //if (order == null)
            //    return false;

            //if (order.LineItems.Count < 1)
            //    return false;

            //if (order.TotalQuantity < 1 || order.TotalAmount < 0)
            //    return false;

            return true;
        }

    }
}