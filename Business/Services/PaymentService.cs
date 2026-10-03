using Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IEnumerable<IPaymentStrategy> _strategies;

        public PaymentService(IEnumerable<IPaymentStrategy> strategies)
        {
            _strategies = strategies;
        }

        public PaymentResult Charge(Customer customer, PoolSession session, DateTime date)
        {
            var strategy = _strategies.First(s => s.CanPay(customer, session, date));
            return strategy.Pay(customer, session, date);
        }
    }
}
