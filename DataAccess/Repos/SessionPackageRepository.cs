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

        public SessionPackage GetActivePackage(int customerId, DateTime date, SessionType type)
        {
            var query = _context.SessionPackages
                .Where(p => p.CustomerId == customerId && p.ExpiryDate >= date);

            if (type == SessionType.Fixed)
                query = query.Where(p => p.FixedUsed < p.FixedTotal);
            else
                query = query.Where(p => p.FreeTimeUsed < p.FreeTimeTotal);

            return query.OrderBy(p => p.ExpiryDate).FirstOrDefault(); 
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
