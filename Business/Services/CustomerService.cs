using Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customers;

        public CustomerService(ICustomerRepository customers)
        {
            _customers = customers;
        }

        public List<Customer> GetAll() => _customers.GetAll();
        public Customer GetById(int id) => _customers.GetById(id);

        public OperationResult Update(int id, string fullName, string email, string phone, bool isVip)
        {
            var customer = _customers.GetById(id);
            if (customer == null) return OperationResult.Fail("Customer not found.");

            var other = _customers.GetByEmail(email);
            if (other != null && other.Id != id)
                return OperationResult.Fail("Another customer already uses this email.");

            customer.FullName = fullName;
            customer.Email = email;
            customer.Phone = phone;
            customer.IsVip = isVip;
            _customers.Update(customer);
            return OperationResult.Ok("Customer updated.");
        }
    }
}
