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
            return  _context.Visits
                //.Include(v => v.Session).ThenInclude(s => s.Pool)
                .Where(v => v.CustomerId == customerId && v.VisitDate >= from).Select(v => new VisitReportItem
                {
                    VisitDate = v.VisitDate,
                    PoolName = v.Session.Pool.Name,
                    SessionText = v.Session.Description
                })
                .OrderBy(v => v.VisitDate)
                .ToList();

            //var result = visits.Select(v=> new VisitReportItem
            //{
            //    VisitDate = v.VisitDate,
            //    PoolName = v.Session.Pool.Name,
            //    SessionText = v.Session.Description
            //}).ToList();
            ////foreach (var v in visits) 
            ////{
            ////    result.Add(new VisitReportItem
            ////    {
            ////        VisitDate = v.VisitDate,
            ////        PoolName = v.Session.Pool.Name,
            ////        SessionText = v.Session.Description
            ////    });
            ////}
            //return visits.Select(v => new VisitReportItem
            //{
            //    VisitDate = v.VisitDate,
            //    PoolName = v.Session.Pool.Name,
            //    SessionText = v.Session.Description
            //}).ToList();
        }

        public List<PoolCountItem> GetPoolVisitCounts(DateTime from, DateTime toExclusive, int top)
        {
            return _context.Visits
                .Where(v => v.VisitDate >= from && v.VisitDate < toExclusive)
                .GroupBy(v => new { v.Session.Pool.Id, v.Session.Pool.Name })
                .OrderByDescending(g => g.Count())
                .ThenBy(g => g.Key.Name)
                .Take(top)
                .Select(g => new PoolCountItem
                {
                    PoolName = g.Key.Name,
                    VisitCount = g.Count()
                })
                .ToList();
        }

        public List<MissedBookingItem> GetMissedBookings(DateTime from, DateTime toExclusive)
        {
            return _context.Bookings
                .Where(b => b.VisitId == null
                         && b.Status != BookingStatus.Cancelled
                         && b.SessionDate >= from && b.SessionDate < toExclusive)
                .OrderBy(b => b.SessionDate)
                .Select(b => new MissedBookingItem
                {
                    CustomerName = b.Customer.FullName,
                    PoolName = b.Session.Pool.Name,
                    Date = b.SessionDate,
                    Day = b.Session.Day,
                    StartTime = b.Session.StartTime,
                    EndTime = b.Session.EndTime,
                    Type = b.Session.Type
                })
                .ToList();
        }
    }
}
