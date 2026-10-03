using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public static OperationResult Ok(string message) => new OperationResult { Success = true, Message = message };
        public static OperationResult Fail(string message) => new OperationResult { Success = false, Message = message };
    }

    public class PaymentResult
    {
        public PaymentMethod Method { get; set; }
        public decimal Amount { get; set; }
        public int? PackageId { get; set; }
    }

    public class PackagePlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FixedSessions { get; set; }
        public int FreeTimeSessions { get; set; }
        public int ValidDays { get; set; }
        public decimal Price { get; set; }
    }

    public class VisitReportItem
    {
        public DateTime VisitDate { get; set; }
        public string PoolName { get; set; }
        public string SessionText { get; set; }
    }

    public class PoolCountItem
    {
        public string PoolName { get; set; }
        public int VisitCount { get; set; }
    }

    public class MissedBookingItem
    {
        public string CustomerName { get; set; }
        public string PoolName { get; set; }
        public DateTime Date { get; set; }
        public string SessionText { get; set; }
    }
}
