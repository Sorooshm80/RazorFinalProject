using Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class VipReservationPolicy : IReservationPolicy
    {
        private readonly ISessionPackageRepository _packages;

        public VipReservationPolicy(ISessionPackageRepository packages)
        {
            _packages = packages;
        }

        public bool CanReserve(Customer customer, PoolSession session, DateTime date)
        {
            var hasCredit = _packages.GetActivePackage(customer.Id, date, session.Type) != null;
            return CanReserve(customer.IsVip, hasCredit);
        }

        public bool CanReserve(bool isVip, bool hasPackageCredit)
        {
            if (!isVip) return true;
            return !hasPackageCredit;
        }
    }
}
