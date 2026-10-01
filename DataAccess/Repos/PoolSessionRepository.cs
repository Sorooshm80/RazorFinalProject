using Business;
using Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repos
{
    public class PoolSessionRepository : Repository<PoolSession>, IPoolSessionRepository
    {
        public PoolSessionRepository(AppDbContext context) : base(context) { }

        public List<PoolSession> GetByPoolAndDay(int poolId, DayOfWeek day)
        {
            return _context.Sessions
                .Where(s => s.PoolId == poolId && s.Day == day)
                .OrderBy(s => s.StartTime)
                .ToList();
        }
    }
}
