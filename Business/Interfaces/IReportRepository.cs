using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IReportRepository
    {
        List<VisitReportItem> GetCustomerVisits(int customerId, DateTime from);
        List<PoolCountItem> GetPoolVisitCounts(DateTime from, DateTime toExclusive);
        List<MissedBookingItem> GetMissedBookings(DateTime from, DateTime toExclusive);
    }
}
