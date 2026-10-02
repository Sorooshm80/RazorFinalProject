using Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class PackageService : IPackageService
    {
        private readonly ICustomerRepository _customers;
        private readonly ISessionPackageRepository _packages;
        private readonly ICommandInvoker _invoker;

        public PackageService(ICustomerRepository customers, ISessionPackageRepository packages,
                              ICommandInvoker invoker)
        {
            _customers = customers;
            _packages = packages;
            _invoker = invoker;
        }

        public List<PackagePlan> GetPlans()
        {
            return new List<PackagePlan>
        {
            new PackagePlan { Id = 1, Name = "Starter",  FixedSessions = 5,  FreeTimeSessions = 0,  ValidDays = 30,  Price = 40 },
            new PackagePlan { Id = 2, Name = "Standard", FixedSessions = 8,  FreeTimeSessions = 4,  ValidDays = 60,  Price = 90 },
            new PackagePlan { Id = 3, Name = "Premium",  FixedSessions = 10, FreeTimeSessions = 10, ValidDays = 120, Price = 150 }
        };
        }

        public List<SessionPackage> GetPackages(int customerId) => _packages.GetByCustomer(customerId);

        public OperationResult Buy(int customerId, int planId)
        {
            var plan = GetPlans().FirstOrDefault(p => p.Id == planId);
            if (plan == null) return OperationResult.Fail("Plan not found.");

            var command = new BuyPackageCommand(_customers, _packages, customerId, plan);
            return _invoker.Run(command);
        }
    }
}
