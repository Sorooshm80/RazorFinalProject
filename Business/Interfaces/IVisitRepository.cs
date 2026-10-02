using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IVisitRepository : IRepository<Visit>
    {
        bool Exists(int customerId, int sessionId, DateTime date);
        List<Visit> GetByCustomerAndDate(int customerId, DateTime date);
    }
}
