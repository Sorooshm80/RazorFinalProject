using Business;
using Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repos
{
    public class PoolSessionRepository : Repository<PoolSession>, IPoolSessionRepository
    {
        public PoolSessionRepository(AppDbContext context) : base(context) { }

        public List<PoolSession> GetByPoolAndDay(int poolId, DayOfWeek day)
        {
            return _context.Sessions
                .Where(s => s.PoolId == poolId && s.Day == day)
                .OrderBy(s => s.StartTime)
                .ToList();
        }

        public List<SessionRowData> GetSessionRowData(int poolId, DateTime date, int customerId)
        {
            var day = date.DayOfWeek;
            var from = date.Date;
            var to = from.AddDays(1);

            return _context.Sessions
                .Where(s => s.PoolId == poolId && s.Day == day)
                .OrderBy(s => s.StartTime)
                .Select(s => new SessionRowData
                {
                    Session = s,

                    HasVisit = _context.Visits.Any(v =>
                        v.CustomerId == customerId && v.SessionId == s.Id
                        && v.VisitDate >= from && v.VisitDate < to),

                    HasReservation = _context.Bookings.Any(b =>
                        b.CustomerId == customerId && b.SessionId == s.Id
                        && b.SessionDate == from && b.Status == BookingStatus.Reserved),

                    HasPackageCredit = _context.SessionPackages.Any(p =>
                        p.CustomerId == customerId && p.ExpiryDate >= from
                        && ((s.Type == SessionType.Fixed && p.FixedUsed < p.FixedTotal)
                         || (s.Type == SessionType.FreeTime && p.FreeTimeUsed < p.FreeTimeTotal))),

                    CustomerIsVip = _context.Customers.Any(c => c.Id == customerId && c.IsVip)
                })
                .ToList();
        }
    }
}
