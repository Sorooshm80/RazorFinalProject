using Business.Interfaces;
using Business.Services;
using DataAccess;
using DataAccess.Repos;
using Microsoft.EntityFrameworkCore;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();
            builder.Services.AddSession();

            // SINGLETON: one instance for the whole app (no state, no scoped dependencies)
            builder.Services.AddSingleton<IPasswordHasher, Sha256PasswordHasher>();
            builder.Services.AddSingleton<ICommandInvoker, CommandInvoker>();
            builder.Services.AddSingleton<IAdminSettings>(
                new AdminSettings { Password = builder.Configuration["AdminPassword"] });

            // SCOPED: one instance per web request (DbContext, repositories, services)
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));  // generic
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IPoolSessionRepository, PoolSessionRepository>();
            builder.Services.AddScoped<ISessionPackageRepository, SessionPackageRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IPoolService, PoolService>();
            builder.Services.AddScoped<IPackageService, PackageService>();
            builder.Services.AddScoped<IReservationService, ReservationService>();
            builder.Services.AddScoped<IEntryService, EntryService>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IVisitRepository, VisitRepository>();

            // TRANSIENT: new instance every time. Order matters: package strategy is tried first.
            builder.Services.AddTransient<IPaymentStrategy, PackagePaymentStrategy>();
            builder.Services.AddTransient<IPaymentStrategy, PayPerEntryStrategy>();

            var app = builder.Build();

            // Create the database from the migration and add the sample pools
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
                DbSeeder.Seed(db);
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.MapRazorPages();
            app.Run();
        }
    }
}
