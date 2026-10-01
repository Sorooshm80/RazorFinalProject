namespace Business
{
    public enum SessionType { Fixed, FreeTime }
    public enum PaymentMethod { Package, PayPerEntry }
    public enum BookingStatus { Reserved, Attended }

    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }

    public class Pool : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public List<PoolSession> Sessions { get; set; } = new List<PoolSession>();
    }

    // One row = one session of one pool on one weekday.
    // Fixed pool: several 2h rows. Free-time pool: one long row.
    // Mixed day: fixed rows + one free-time row.
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
        public int TotalSessions { get; set; }
        public int UsedSessions { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal Price { get; set; }

        public int RemainingSessions => TotalSessions - UsedSessions;
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
