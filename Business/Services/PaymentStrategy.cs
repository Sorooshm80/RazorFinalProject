using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces;

namespace Business.Services
{
    public class PackagePaymentStrategy : IPaymentStrategy
    {
        private readonly ISessionPackageRepository _packages;

        public PackagePaymentStrategy(ISessionPackageRepository packages)
        {
            _packages = packages;
        }

        public bool CanPay(Customer customer, PoolSession session, DateTime date)
        {
            return _packages.GetActivePackage(customer.Id, date, session.Type) != null;
        }

        public PaymentResult Pay(Customer customer, PoolSession session, DateTime date)
        {
            var package = _packages.GetActivePackage(customer.Id, date, session.Type);

            if (session.Type == SessionType.Fixed)
                package.FixedUsed++;
            else
                package.FreeTimeUsed++;

            _packages.Update(package);
            return new PaymentResult { Method = PaymentMethod.Package, Amount = 0, PackageId = package.Id };
        }
    }

    public class PayPerEntryStrategy : IPaymentStrategy
    {
        public bool CanPay(Customer customer, PoolSession session, DateTime date) => true;

        public PaymentResult Pay(Customer customer, PoolSession session, DateTime date)
        {
            return new PaymentResult { Method = PaymentMethod.PayPerEntry, Amount = session.Price, PackageId = null };
        }
    }

    

    
}
