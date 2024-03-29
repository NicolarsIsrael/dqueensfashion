﻿using DQueensFashion.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Data.Contract
{
    public interface IOrderRepo : ICoreRepo<Order>
    {
        IEnumerable<Order> GetAllOrdersWithRelationships();
        Order GetOrdertByIdWithRelationships(int orderId);
    }
}
