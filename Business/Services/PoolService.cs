using Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class PoolService : IPoolService
    {
        private readonly IRepository<Pool> _pools;
        private readonly IPoolSessionRepository _sessions;
        private readonly IBookingRepository _bookings;
        private readonly IVisitRepository _visits;

        public PoolService(IRepository<Pool> pools, IPoolSessionRepository sessions,
                           IBookingRepository bookings, IVisitRepository visits)
        {
            _pools = pools;
            _sessions = sessions;
            _bookings = bookings;
            _visits = visits;
        }

        public List<Pool> GetPools() => _pools.GetAll();

        public List<SessionRow> GetSessionRows(int poolId, DateTime date, int customerId)
        {
            var sessions = _sessions.GetByPoolAndDay(poolId, date.DayOfWeek);
            var bookings = _bookings.GetByCustomerAndDate(customerId, date.Date);
            var visits = _visits.GetByCustomerAndDate(customerId, date.Date);

            var rows = new List<SessionRow>();
            foreach (var s in sessions)
            {
                var state = SessionState.Available;
                if (visits.Any(v => v.SessionId == s.Id))
                    state = SessionState.Entered;
                else if (bookings.Any(b => b.SessionId == s.Id && b.Status == BookingStatus.Reserved))
                    state = SessionState.Reserved;

                rows.Add(new SessionRow { Session = s, State = state });
            }
            return rows;
        }
    }
}
