using Business;
using Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repos
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context) { }

        public Customer GetByEmail(string email)
        {
            return _context.Customers.FirstOrDefault(c => c.Email == email);
        }
    }
}
