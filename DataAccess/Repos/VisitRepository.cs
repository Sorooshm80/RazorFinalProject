using Business;
using Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repos
{
    public class VisitRepository : Repository<Visit>, IVisitRepository
    {
        public VisitRepository(AppDbContext context) : base(context) { }

        public bool Exists(int customerId, int sessionId, DateTime date)
        {
            var from = date.Date;
            var to = from.AddDays(1);
            return _context.Visits.Any(v => v.CustomerId == customerId && v.SessionId == sessionId
                                         && v.VisitDate >= from && v.VisitDate < to);
        }

        public List<Visit> GetByCustomerAndDate(int customerId, DateTime date)
        {
            var from = date.Date;
            var to = from.AddDays(1);
            return _context.Visits
                .Where(v => v.CustomerId == customerId && v.VisitDate >= from && v.VisitDate < to)
                .ToList();
        }
    }
}
