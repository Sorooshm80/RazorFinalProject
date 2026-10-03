using Business;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pool> Pools { get; set; }
        public DbSet<PoolSession> Sessions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SessionPackage> SessionPackages { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Visit> Visits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasIndex(c => c.Email).IsUnique();

            modelBuilder.Entity<PoolSession>().Property(x => x.Price).HasPrecision(18, 2);
            modelBuilder.Entity<SessionPackage>().Property(x => x.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Booking>().Property(x => x.AmountPaid).HasPrecision(18, 2);
            modelBuilder.Entity<Visit>().Property(x => x.AmountPaid).HasPrecision(18, 2);
            modelBuilder.Entity<Booking>()
            .HasOne(b => b.Visit)
            .WithOne()
            .HasForeignKey<Booking>(b => b.VisitId)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
