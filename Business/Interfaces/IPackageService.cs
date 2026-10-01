using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IPackageService
    {
        List<PackagePlan> GetPlans();
        List<SessionPackage> GetPackages(int customerId);
        OperationResult Buy(int customerId, int planId);
    }
}
