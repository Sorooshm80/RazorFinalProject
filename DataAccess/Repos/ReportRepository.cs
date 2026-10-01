using Business;
using Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repos
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<VisitReportItem> GetCustomerVisits(int customerId, DateTime from)
        {
            var visits = _context.Visits
                .Include(v => v.Session).ThenInclude(s => s.Pool)
                .Where(v => v.CustomerId == customerId && v.VisitDate >= from)
                .OrderBy(v => v.VisitDate)
                .ToList();

            var result = new List<VisitReportItem>();
            foreach (var v in visits)
            {
                result.Add(new VisitReportItem
                {
                    VisitDate = v.VisitDate,
                    PoolName = v.Session.Pool.Name,
                    SessionText = v.Session.Description
                });
            }
            return result;
        }

        // Used by report 2 and report 3. Sorted: busiest pool first.
        public List<PoolCountItem> GetPoolVisitCounts(DateTime from, DateTime toExclusive)
        {
            var visits = _context.Visits
                .Include(v => v.Session).ThenInclude(s => s.Pool)
                .Where(v => v.VisitDate >= from && v.VisitDate < toExclusive)
                .ToList();

            return visits
                .GroupBy(v => v.Session.Pool.Name)
                .Select(g => new PoolCountItem { PoolName = g.Key, VisitCount = g.Count() })
                .OrderByDescending(x => x.VisitCount)
                .ToList();
        }

        public List<MissedBookingItem> GetMissedBookings(DateTime from, DateTime toExclusive)
        {
            var bookings = _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Session).ThenInclude(s => s.Pool)
                .Where(b => b.Status == BookingStatus.Reserved
                         && b.SessionDate >= from && b.SessionDate < toExclusive)
                .OrderBy(b => b.SessionDate)
                .ToList();

            var result = new List<MissedBookingItem>();
            foreach (var b in bookings)
            {
                result.Add(new MissedBookingItem
                {
                    CustomerName = b.Customer.FullName,
                    PoolName = b.Session.Pool.Name,
                    Date = b.SessionDate,
                    SessionText = b.Session.Description
                });
            }
            return result;
        }
    }
}
