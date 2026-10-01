using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ISessionPackageRepository : IRepository<SessionPackage>
    {
        SessionPackage GetActivePackage(int customerId, DateTime today);
        List<SessionPackage> GetByCustomer(int customerId);
    }
}
