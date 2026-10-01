using Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    // One place to catch errors for every command
    public class CommandInvoker : ICommandInvoker
    {
        public OperationResult Run(ICommand command)
        {
            try
            {
                return command.Execute();
            }
            catch (Exception ex)
            {
                return OperationResult.Fail("Something went wrong: " + ex.Message);
            }
        }
    }

    public class BuyPackageCommand : ICommand
    {
        private readonly ICustomerRepository _customers;
        private readonly ISessionPackageRepository _packages;
        private readonly int _customerId;
        private readonly PackagePlan _plan;

        public BuyPackageCommand(ICustomerRepository customers, ISessionPackageRepository packages,
                                 int customerId, PackagePlan plan)
        {
            _customers = customers;
            _packages = packages;
            _customerId = customerId;
            _plan = plan;
        }

        public OperationResult Execute()
        {
            var customer = _customers.GetById(_customerId);
            if (customer == null)
                return OperationResult.Fail("Customer not found.");

            var package = new SessionPackage
            {
                CustomerId = customer.Id,
                TotalSessions = _plan.Sessions,
                UsedSessions = 0,
                PurchaseDate = DateTime.Today,
                ExpiryDate = DateTime.Today.AddDays(_plan.ValidDays),
                Price = _plan.Price
            };
            _packages.Add(package);

            customer.IsVip = true;          // buying a package = VIP
            _customers.Update(customer);

            return OperationResult.Ok("Package purchased. You are now a VIP!");
        }
    }

    // Reserving also pays (with package or normal price), so entry later is free
    public class ReserveSessionCommand : ICommand
    {
        private readonly Customer _customer;
        private readonly PoolSession _session;
        private readonly DateTime _date;
        private readonly IBookingRepository _bookings;
        private readonly IPaymentService _payments;

        public ReserveSessionCommand(Customer customer, PoolSession session, DateTime date,
                                     IBookingRepository bookings, IPaymentService payments)
        {
            _customer = customer;
            _session = session;
            _date = date;
            _bookings = bookings;
            _payments = payments;
        }

        public OperationResult Execute()
        {
            if (_date < DateTime.Today)
                return OperationResult.Fail("You cannot reserve a session in the past.");
            if (_date.DayOfWeek != _session.Day)
                return OperationResult.Fail("This session does not run on the chosen date.");
            if (_bookings.Find(_customer.Id, _session.Id, _date) != null)
                return OperationResult.Fail("You already reserved this session.");

            var payment = _payments.Charge(_customer, _session);

            var booking = new Booking
            {
                CustomerId = _customer.Id,
                SessionId = _session.Id,
                SessionDate = _date,
                Status = BookingStatus.Reserved,
                PaymentMethod = payment.Method,
                AmountPaid = payment.Amount
            };
            _bookings.Add(booking);

            return OperationResult.Ok("Session reserved.");
        }
    }

    public class EnterPoolCommand : ICommand
    {
        private readonly Customer _customer;
        private readonly PoolSession _session;
        private readonly IBookingRepository _bookings;
        private readonly IRepository<Visit> _visits;
        private readonly IPaymentService _payments;

        public EnterPoolCommand(Customer customer, PoolSession session, IBookingRepository bookings,
                                IRepository<Visit> visits, IPaymentService payments)
        {
            _customer = customer;
            _session = session;
            _bookings = bookings;
            _visits = visits;
            _payments = payments;
        }

        public OperationResult Execute()
        {
            var today = DateTime.Today;
            if (_session.Day != today.DayOfWeek)
                return OperationResult.Fail("This session is not for today.");

            var booking = _bookings.Find(_customer.Id, _session.Id, today);
            PaymentMethod method;
            decimal amount;

            if (booking != null && booking.Status == BookingStatus.Attended)
            {
                return OperationResult.Fail("You already entered with this reservation.");
            }
            else if (booking != null)
            {
                // Already paid when reserving
                booking.Status = BookingStatus.Attended;
                _bookings.Update(booking);
                method = booking.PaymentMethod;
                amount = 0;
            }
            else
            {
                var payment = _payments.Charge(_customer, _session);
                method = payment.Method;
                amount = payment.Amount;
            }

            var visit = new Visit
            {
                CustomerId = _customer.Id,
                SessionId = _session.Id,
                VisitDate = DateTime.Now,
                PaymentMethod = method,
                AmountPaid = amount
            };
            _visits.Add(visit);

            return OperationResult.Ok("Welcome to the pool!");
        }
    }
}
