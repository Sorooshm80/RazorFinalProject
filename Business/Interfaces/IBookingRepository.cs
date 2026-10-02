using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Booking Find(int customerId, int sessionId, DateTime date);   // latest booking
        List<Booking> GetByCustomer(int customerId);
        List<Booking> GetByCustomerAndDate(int customerId, DateTime date);
    }
}
