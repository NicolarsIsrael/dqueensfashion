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
                if (p.CategoryId == AppConstant.ReadyMadeCategoryId)
                {
                    if (string.Compare(lineItem.Description,AppConstant.ReadyMadeSizes[0],true)==0)
                        p.ExtraSmallQuantity = p.ExtraSmallQuantity - lineItem.Quantity < 0 ? 0 : p.ExtraSmallQuantity - lineItem.Quantity;

                    if (string.Compare(lineItem.Description, AppConstant.ReadyMadeSizes[1], true) == 0)
                        p.SmallQuantiy = p.SmallQuantiy - lineItem.Quantity < 0 ? 0 : p.SmallQuantiy - lineItem.Quantity;

                    if (string.Compare(lineItem.Description, AppConstant.ReadyMadeSizes[2], true) == 0)
                        p.MediumQuantiy = p.MediumQuantiy - lineItem.Quantity < 0 ? 0 : p.MediumQuantiy - lineItem.Quantity;

                    if (string.Compare(lineItem.Description, AppConstant.ReadyMadeSizes[3], true) == 0)
                        p.LargeQuantity = p.LargeQuantity - lineItem.Quantity < 0 ? 0 : p.LargeQuantity - lineItem.Quantity;

                    if (string.Compare(lineItem.Description, AppConstant.ReadyMadeSizes[4], true) == 0)
                        p.ExtraLargeQuantity = p.ExtraLargeQuantity - lineItem.Quantity < 0 ? 0 : p.ExtraLargeQuantity - lineItem.Quantity;
                }
                else if(p.CategoryId != AppConstant.CustomMadeCategoryId)
                {
                    p.Quantity -= lineItem.Quantity;
                }
            }

            uow.OrderRepo.Add(order);
            uow.Save();
        }

        public IEnumerable<Order> GetAllOrdersForCustomer(int customerId)
        {
            return uow.OrderRepo.GetAllOrdersWithRelationships().Where(order=>order.CustomerId==customerId);
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