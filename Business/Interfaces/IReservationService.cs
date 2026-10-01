using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IReservationService
    {
        OperationResult Reserve(int customerId, int sessionId, DateTime date);
        List<Booking> GetBookings(int customerId);
    }
}
