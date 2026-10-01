using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IPoolService
    {
        List<Pool> GetPools();
        List<PoolSession> GetSessions(int poolId, DateTime date);
    }
}
