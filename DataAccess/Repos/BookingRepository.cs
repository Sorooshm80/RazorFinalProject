using Business;
using Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repos
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        public BookingRepository(AppDbContext context) : base(context) { }

        public Booking Find(int customerId, int sessionId, DateTime date)
        {
            return _context.Bookings.FirstOrDefault(b =>
                b.CustomerId == customerId && b.SessionId == sessionId && b.SessionDate == date);
        }

        public List<Booking> GetByCustomer(int customerId)
        {
            return _context.Bookings
                .Include(b => b.Session).ThenInclude(s => s.Pool)
                .Where(b => b.CustomerId == customerId)
                .OrderByDescending(b => b.SessionDate)
                .ToList();
        }
    }
}
