﻿using DQueensFashion.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Contract
{
    public interface ICustomerService
    {
        int GetAllCustomerCount();
        void AddCustomer(Customer customer);
        Customer GedCustomerByUserId(string userId);
    }
}
