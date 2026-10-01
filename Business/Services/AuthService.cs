using Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly ICustomerRepository _customers;
        private readonly IPasswordHasher _hasher;
        private readonly IAdminSettings _admin;

        public AuthService(ICustomerRepository customers, IPasswordHasher hasher, IAdminSettings admin)
        {
            _customers = customers;
            _hasher = hasher;
            _admin = admin;
        }

        public OperationResult SignUp(string fullName, string email, string phone, string password)
        {
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
                return OperationResult.Fail("Name, email and password are required.");

            if (_customers.GetByEmail(email) != null)
                return OperationResult.Fail("This email is already registered.");

            var customer = new Customer
            {
                FullName = fullName,
                Email = email,
                Phone = phone,
                PasswordHash = _hasher.Hash(password),
                IsVip = false
            };
            _customers.Add(customer);
            return OperationResult.Ok("Account created. You can log in now.");
        }

        public Customer Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            var customer = _customers.GetByEmail(email);
            if (customer == null) return null;
            if (customer.PasswordHash != _hasher.Hash(password)) return null;
            return customer;
        }

        public bool AdminLogin(string password)
        {
            return password == _admin.Password;
        }
    }
}
