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

        public PoolService(IRepository<Pool> pools, IPoolSessionRepository sessions)
        {
            _pools = pools;
            _sessions = sessions;
        }

        public List<Pool> GetPools() => _pools.GetAll();

        public List<PoolSession> GetSessions(int poolId, DateTime date)
        {
            return _sessions.GetByPoolAndDay(poolId, date.DayOfWeek);
        }
    }
}
