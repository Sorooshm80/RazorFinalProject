using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IPoolSessionRepository : IRepository<PoolSession>
    {
        List<PoolSession> GetByPoolAndDay(int poolId, DayOfWeek day);
        List<SessionRowData> GetSessionRowData(int poolId, DateTime date, int customerId);
    }
}
