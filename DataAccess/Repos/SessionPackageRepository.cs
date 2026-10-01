using Business;
using Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repos
{
    public class SessionPackageRepository : Repository<SessionPackage>, ISessionPackageRepository
    {
        public SessionPackageRepository(AppDbContext context) : base(context) { }

        public SessionPackage GetActivePackage(int customerId, DateTime today)
        {
            return _context.SessionPackages
                .Where(p => p.CustomerId == customerId
                         && p.ExpiryDate >= today
                         && p.UsedSessions < p.TotalSessions)
                .OrderBy(p => p.ExpiryDate)      // use the one expiring first
                .FirstOrDefault();
        }

        public List<SessionPackage> GetByCustomer(int customerId)
        {
            return _context.SessionPackages
                .Where(p => p.CustomerId == customerId)
                .OrderByDescending(p => p.PurchaseDate)
                .ToList();
        }
    }
}
