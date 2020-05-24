using DQueensFashion.Data.Contract;
using DQueensFashion.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Implementation
{
    public class CustomerService :ICustomerService
    {
        IUnitOfWork uow;
        public CustomerService(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public int GetAllCustomerCount()
        {
            return uow.CustomerRepo.Count();
        }
    }
}
