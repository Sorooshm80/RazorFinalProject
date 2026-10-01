using Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ICustomerRepository _customers;
        private readonly IPoolSessionRepository _sessions;
        private readonly IBookingRepository _bookings;
        private readonly IPaymentService _payments;
        private readonly ICommandInvoker _invoker;

        public ReservationService(ICustomerRepository customers, IPoolSessionRepository sessions,
                                  IBookingRepository bookings, IPaymentService payments,
                                  ICommandInvoker invoker)
        {
            _customers = customers;
            _sessions = sessions;
            _bookings = bookings;
            _payments = payments;
            _invoker = invoker;
        }

        public OperationResult Reserve(int customerId, int sessionId, DateTime date)
        {
            var customer = _customers.GetById(customerId);
            var session = _sessions.GetById(sessionId);
            if (customer == null || session == null)
                return OperationResult.Fail("Customer or session not found.");

            var command = new ReserveSessionCommand(customer, session, date.Date, _bookings, _payments);
            return _invoker.Run(command);
        }

        public List<Booking> GetBookings(int customerId) => _bookings.GetByCustomer(customerId);
    }
}
