using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ICustomerService
    {
        List<Customer> GetAll();
        Customer GetById(int id);
        OperationResult Update(int id, string fullName, string email, string phone, bool isVip);
    }
}
