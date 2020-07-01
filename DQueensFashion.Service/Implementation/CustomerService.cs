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

        public IEnumerable<Customer> GetAllCustomers()
        {
            return uow.CustomerRepo.GetAll();
        }

        public void AddCustomer(Customer customer)
        {
            if (!ValidateCustomerDetails(customer))
                throw new Exception();

            uow.CustomerRepo.Add(customer);
            uow.Save();
        }

        public Customer GetCustomerById(int id)
        {
            return uow.CustomerRepo.Get(id);
        }

        private bool ValidateCustomerDetails(Customer customer)
        {
            if (customer == null)
                throw new Exception();

            if (string.IsNullOrEmpty(customer.UserId) || string.IsNullOrWhiteSpace(customer.UserId))
                return false;

            return true;
        }

        public Customer GedCustomerByUserId(string userId)
        {
            return uow.CustomerRepo.GetAll().Where(c => string.Compare(c.UserId, userId, false) == 0).FirstOrDefault();
        }
    }
}
