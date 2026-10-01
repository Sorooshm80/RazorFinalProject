using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IReportService
    {
        List<VisitReportItem> CustomerVisits(int customerId, int days);
        List<PoolCountItem> TopPools(int days, int top);
        PoolCountItem BusiestPoolOnDate(DateTime date);
        List<MissedBookingItem> MissedBookings(int days);
    }
}
