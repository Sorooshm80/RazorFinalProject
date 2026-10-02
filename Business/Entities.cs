namespace Business
{
    public enum SessionType { Fixed, FreeTime }
    public enum PaymentMethod { Package, PayPerEntry }
    public enum BookingStatus { Reserved, Attended, Cancelled }
    public enum SessionState { Available, Reserved, Entered }

    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }

    public class SessionRow
    {
        public PoolSession Session { get; set; }
        public SessionState State { get; set; }
    }
    public class Pool : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public List<PoolSession> Sessions { get; set; } = new List<PoolSession>();
    }

    public class PoolSession : BaseEntity
    {
        public int PoolId { get; set; }
        public Pool Pool { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public SessionType Type { get; set; }
        public decimal Price { get; set; }

        public string Description => $"{Day} {StartTime:hh\\:mm}-{EndTime:hh\\:mm} ({Type})";
    }

    public class Customer : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public bool IsVip { get; set; }
    }

    public class SessionPackage : BaseEntity
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int FixedTotal { get; set; }
        public int FixedUsed { get; set; }
        public int FreeTimeTotal { get; set; }
        public int FreeTimeUsed { get; set; }

        public DateTime PurchaseDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal Price { get; set; }

        public int FixedRemaining => FixedTotal - FixedUsed;
        public int FreeTimeRemaining => FreeTimeTotal - FreeTimeUsed;
        public bool IsExpired => ExpiryDate.Date < DateTime.Today;
        public int DaysRemaining => IsExpired ? 0 : (ExpiryDate.Date - DateTime.Today).Days;
    }


    public class Booking : BaseEntity
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int SessionId { get; set; }
        public PoolSession Session { get; set; }
        public DateTime SessionDate { get; set; }
        public BookingStatus Status { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal AmountPaid { get; set; }
        public int? SessionPackageId { get; set; }    
    }

    public class Visit : BaseEntity
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int SessionId { get; set; }
        public PoolSession Session { get; set; }
        public DateTime VisitDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal AmountPaid { get; set; }
    }
}
