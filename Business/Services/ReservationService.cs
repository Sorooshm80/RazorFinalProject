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
        private readonly IVisitRepository _visits;
        private readonly ISessionPackageRepository _packages;
        private readonly IPaymentService _payments;
        private readonly ICommandInvoker _invoker;
        private readonly IReservationPolicy _policy;

        public ReservationService(ICustomerRepository customers, IPoolSessionRepository sessions,
                          IBookingRepository bookings, IVisitRepository visits,
                          ISessionPackageRepository packages, IPaymentService payments,
                          ICommandInvoker invoker, IReservationPolicy policy)   
        {
            _customers = customers;
            _sessions = sessions;
            _bookings = bookings;
            _visits = visits;
            _packages = packages;
            _payments = payments;
            _invoker = invoker;
            _policy = policy;  
        }

        public OperationResult Reserve(int customerId, int sessionId, DateTime date)
        {
            var customer = _customers.GetById(customerId);
            var session = _sessions.GetById(sessionId);
            if (customer == null || session == null)
                return OperationResult.Fail("Customer or session not found.");

            var command = new ReserveSessionCommand(customer, session, date.Date, _bookings, _visits, _payments, _policy);
            return _invoker.Run(command);
        }

        public OperationResult Cancel(int customerId, int sessionId, DateTime date)
        {
            var session = _sessions.GetById(sessionId);
            if (session == null)
                return OperationResult.Fail("Session not found.");

            var command = new CancelReservationCommand(customerId, session, date.Date, _bookings, _packages);
            return _invoker.Run(command);
        }

        public List<Booking> GetBookings(int customerId) => _bookings.GetByCustomer(customerId);
    }
}
