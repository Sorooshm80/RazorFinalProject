using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IPaymentStrategy
    {
        bool CanPay(Customer customer);
        PaymentResult Pay(Customer customer, PoolSession session);
    }
}
