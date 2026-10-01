using Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    // Uses one session from the customer's valid package
    public class PackagePaymentStrategy : IPaymentStrategy
    {
        private readonly ISessionPackageRepository _packages;

        public PackagePaymentStrategy(ISessionPackageRepository packages)
        {
            _packages = packages;
        }

        public bool CanPay(Customer customer)
        {
            return _packages.GetActivePackage(customer.Id, DateTime.Today) != null;
        }

        public PaymentResult Pay(Customer customer, PoolSession session)
        {
            var package = _packages.GetActivePackage(customer.Id, DateTime.Today);
            package.UsedSessions++;
            _packages.Update(package);
            return new PaymentResult { Method = PaymentMethod.Package, Amount = 0 };
        }
    }

    // Normal customer pays the session price
    public class PayPerEntryStrategy : IPaymentStrategy
    {
        public bool CanPay(Customer customer) => true;

        public PaymentResult Pay(Customer customer, PoolSession session)
        {
            return new PaymentResult { Method = PaymentMethod.PayPerEntry, Amount = session.Price };
        }
    }

    public interface IPaymentService
    {
        PaymentResult Charge(Customer customer, PoolSession session);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IEnumerable<IPaymentStrategy> _strategies;

        public PaymentService(IEnumerable<IPaymentStrategy> strategies)
        {
            _strategies = strategies;
        }

        public PaymentResult Charge(Customer customer, PoolSession session)
        {
            // The first strategy that can pay wins.
            // The registration order in Program.cs matters: package first.
            var strategy = _strategies.First(s => s.CanPay(customer));
            return strategy.Pay(customer, session);
        }
    }
}
