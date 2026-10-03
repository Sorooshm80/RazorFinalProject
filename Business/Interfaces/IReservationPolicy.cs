using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IReservationPolicy
    {
        bool CanReserve(Customer customer, PoolSession session, DateTime date);
        bool CanReserve(bool isVip, bool hasPackageCredit);
    }
}
