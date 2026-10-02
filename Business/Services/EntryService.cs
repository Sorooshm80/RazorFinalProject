using Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class EntryService : IEntryService
    {
        private readonly ICustomerRepository _customers;
        private readonly IPoolSessionRepository _sessions;
        private readonly IBookingRepository _bookings;
        private readonly IVisitRepository _visits;
        private readonly IPaymentService _payments;
        private readonly ICommandInvoker _invoker;

        public EntryService(ICustomerRepository customers, IPoolSessionRepository sessions,
                            IBookingRepository bookings, IVisitRepository visits,
                            IPaymentService payments, ICommandInvoker invoker)
        {
            _customers = customers;
            _sessions = sessions;
            _bookings = bookings;
            _visits = visits;
            _payments = payments;
            _invoker = invoker;
        }

        public OperationResult Enter(int customerId, int sessionId)
        {
            var customer = _customers.GetById(customerId);
            var session = _sessions.GetById(sessionId);
            if (customer == null || session == null)
                return OperationResult.Fail("Customer or session not found.");

            var command = new EnterPoolCommand(customer, session, _bookings, _visits, _payments);
            return _invoker.Run(command);
        }
    }

}
