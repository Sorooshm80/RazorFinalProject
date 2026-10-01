using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAuthService
    {
        OperationResult SignUp(string fullName, string email, string phone, string password);
        Customer Login(string email, string password);
        bool AdminLogin(string password);
    }
}
