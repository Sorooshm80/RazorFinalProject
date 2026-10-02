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
                FixedTotal = _plan.FixedSessions,
                FixedUsed = 0,
                FreeTimeTotal = _plan.FreeTimeSessions,
                FreeTimeUsed = 0,
                PurchaseDate = DateTime.Today,
                ExpiryDate = DateTime.Today.AddDays(_plan.ValidDays),
                Price = _plan.Price
            };
            _packages.Add(package);

            customer.IsVip = true;
            _customers.Update(customer);

            return OperationResult.Ok("Package purchased. You are now a VIP!");
        }
    }

    public class ReserveSessionCommand : ICommand
    {
        private readonly Customer _customer;
        private readonly PoolSession _session;
        private readonly DateTime _date;
        private readonly IBookingRepository _bookings;
        private readonly IVisitRepository _visits;
        private readonly IPaymentService _payments;

        public ReserveSessionCommand(Customer customer, PoolSession session, DateTime date,
                                     IBookingRepository bookings, IVisitRepository visits,
                                     IPaymentService payments)
        {
            _customer = customer;
            _session = session;
            _date = date;
            _bookings = bookings;
            _visits = visits;
            _payments = payments;
        }

        public OperationResult Execute()
        {
            if (_date < DateTime.Today)
                return OperationResult.Fail("You cannot reserve a session in the past.");
            if (_date.DayOfWeek != _session.Day)
                return OperationResult.Fail("This session does not run on the chosen date.");
            if (_visits.Exists(_customer.Id, _session.Id, _date))
                return OperationResult.Fail("You already entered this session.");

            var existing = _bookings.Find(_customer.Id, _session.Id, _date);
            if (existing != null && existing.Status != BookingStatus.Cancelled)
                return OperationResult.Fail("You already reserved this session. You can cancel it or enter.");

            var payment = _payments.Charge(_customer, _session, _date);

            var booking = new Booking
            {
                CustomerId = _customer.Id,
                SessionId = _session.Id,
                SessionDate = _date,
                Status = BookingStatus.Reserved,
                PaymentMethod = payment.Method,
                AmountPaid = payment.Amount,
                SessionPackageId = payment.PackageId
            };
            _bookings.Add(booking);

            return OperationResult.Ok("Session reserved.");
        }
    }

    public class CancelReservationCommand : ICommand
    {
        private readonly int _customerId;
        private readonly PoolSession _session;
        private readonly DateTime _date;
        private readonly IBookingRepository _bookings;
        private readonly ISessionPackageRepository _packages;

        public CancelReservationCommand(int customerId, PoolSession session, DateTime date,
                                        IBookingRepository bookings, ISessionPackageRepository packages)
        {
            _customerId = customerId;
            _session = session;
            _date = date;
            _bookings = bookings;
            _packages = packages;
        }

        public OperationResult Execute()
        {
            var booking = _bookings.Find(_customerId, _session.Id, _date);
            if (booking == null || booking.Status != BookingStatus.Reserved)
                return OperationResult.Fail("There is no active reservation to cancel.");
            if (_date < DateTime.Today)
                return OperationResult.Fail("You cannot cancel a past reservation.");

            booking.Status = BookingStatus.Cancelled;
            _bookings.Update(booking);

            // If a package paid for it, give the session back to the package
            if (booking.PaymentMethod == PaymentMethod.Package && booking.SessionPackageId != null)
            {
                var package = _packages.GetById(booking.SessionPackageId.Value);
                if (package != null)
                {
                    if (_session.Type == SessionType.Fixed && package.FixedUsed > 0)
                        package.FixedUsed--;
                    else if (_session.Type == SessionType.FreeTime && package.FreeTimeUsed > 0)
                        package.FreeTimeUsed--;
                    _packages.Update(package);
                }
                return OperationResult.Ok("Reservation cancelled. The session was returned to your package.");
            }

            return OperationResult.Ok("Reservation cancelled.");
        }
    }

    public class EnterPoolCommand : ICommand
    {
        private readonly Customer _customer;
        private readonly PoolSession _session;
        private readonly IBookingRepository _bookings;
        private readonly IVisitRepository _visits;
        private readonly IPaymentService _payments;

        public EnterPoolCommand(Customer customer, PoolSession session, IBookingRepository bookings,
                                IVisitRepository visits, IPaymentService payments)
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
            if (_visits.Exists(_customer.Id, _session.Id, today))
                return OperationResult.Fail("You already entered this session today.");

            var booking = _bookings.Find(_customer.Id, _session.Id, today);
            PaymentMethod method;
            decimal amount;

            if (booking != null && booking.Status == BookingStatus.Reserved)
            {
                // Already paid when reserving
                booking.Status = BookingStatus.Attended;
                _bookings.Update(booking);
                method = booking.PaymentMethod;
                amount = 0;
            }
            else
            {
                var payment = _payments.Charge(_customer, _session, today);
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
