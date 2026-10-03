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
        private readonly IReservationPolicy _policy;

        public PoolService(IRepository<Pool> pools, IPoolSessionRepository sessions,
                           IReservationPolicy policy)
        {
            _pools = pools;
            _sessions = sessions;
            _policy = policy;
        }

        public List<Pool> GetPools() => _pools.GetAll();

        public List<SessionRow> GetSessionRows(int poolId, DateTime date, int customerId)
        {
            var data = _sessions.GetSessionRowData(poolId, date.Date, customerId);

            var rows = new List<SessionRow>();
            foreach (var d in data)
            {
                var state = SessionState.Available;
                if (d.HasVisit)
                    state = SessionState.Entered;
                else if (d.HasReservation)
                    state = SessionState.Reserved;

                rows.Add(new SessionRow
                {
                    Session = d.Session,
                    State = state,
                    CanReserve = _policy.CanReserve(d.CustomerIsVip, d.HasPackageCredit)
                });
            }
            return rows;
        }
    }
}
