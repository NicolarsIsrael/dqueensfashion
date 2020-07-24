using DQueensFashion.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Contract
{
    public interface IOrderService
    {
        int GetOrderCount();
        void CreateOrder(Order order);
        IEnumerable<Order> GetAllOrders();
        IEnumerable<Order> GetProcessingOrders();
        IEnumerable<Order> GetDeliveredOrders();
        IEnumerable<Order> GetReturnedOrders();
       // IEnumerable<Order> GetDeletedOrders();
        IEnumerable<Order> GetCompletedOrders();
        IEnumerable<Order> GetAllOrdersForCustomer(int customerId);
        Order GetOrderById(int id);
        void UpdateOrder(Order order);
        IEnumerable<Order> GetInTransitOrders();
    }
}
