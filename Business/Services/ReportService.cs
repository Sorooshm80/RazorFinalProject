using Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reports;

        public ReportService(IReportRepository reports)
        {
            _reports = reports;
        }

        // Report 1
        public List<VisitReportItem> CustomerVisits(int customerId, int days)
        {
            return _reports.GetCustomerVisits(customerId, DateTime.Today.AddDays(-days));
        }

        // Report 2
        public List<PoolCountItem> TopPools(int days, int top)
        {
            var from = DateTime.Today.AddDays(-days);
            var to = DateTime.Today.AddDays(1);
            return _reports.GetPoolVisitCounts(from, to).Take(top).ToList();
        }

        // Report 3
        public PoolCountItem BusiestPoolOnDate(DateTime date)
        {
            var counts = _reports.GetPoolVisitCounts(date.Date, date.Date.AddDays(1));
            return counts.FirstOrDefault();   // list is sorted, highest first
        }

        // Report 4 (today is excluded because today's sessions may not be over yet)
        public List<MissedBookingItem> MissedBookings(int days)
        {
            return _reports.GetMissedBookings(DateTime.Today.AddDays(-days), DateTime.Today);
        }
    }
}
