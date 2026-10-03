using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            if (db.Pools.Any()) return;

            var fixedPool = new Pool { Name = "Azadi Pool (fixed sessions)", Address = "Street 1" };
            var freePool = new Pool { Name = "Lake Pool (free time)", Address = "Street 2" };
            var mixedPool = new Pool { Name = "Central Pool (mixed)", Address = "Street 3" };
            db.Pools.AddRange(fixedPool, freePool, mixedPool);

            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {

                db.Sessions.Add(MakeSession(fixedPool, day, 8, 10, SessionType.Fixed, 10));
                db.Sessions.Add(MakeSession(fixedPool, day, 10, 12, SessionType.Fixed, 10));
                db.Sessions.Add(MakeSession(fixedPool, day, 14, 16, SessionType.Fixed, 10));
                db.Sessions.Add(MakeSession(fixedPool, day, 16, 18, SessionType.Fixed, 10));


                db.Sessions.Add(MakeSession(freePool, day, 9, 21, SessionType.FreeTime, 8));
                
                if (day == DayOfWeek.Sunday)
                {
                    db.Sessions.Add(MakeSession(mixedPool, day, 8, 10, SessionType.Fixed, 10));
                    db.Sessions.Add(MakeSession(mixedPool, day, 10, 12, SessionType.Fixed, 10));
                    db.Sessions.Add(MakeSession(mixedPool, day, 12, 20, SessionType.FreeTime, 8));
                }
                else
                {
                    db.Sessions.Add(MakeSession(mixedPool, day, 9, 20, SessionType.FreeTime, 8));
                }
            }

            db.SaveChanges();
        }

        private static PoolSession MakeSession(Pool pool, DayOfWeek day, int startHour, int endHour,
                                               SessionType type, decimal price)
        {
            return new PoolSession
            {
                Pool = pool,
                Day = day,
                StartTime = TimeSpan.FromHours(startHour),
                EndTime = TimeSpan.FromHours(endHour),
                Type = type,
                Price = price
            };
        }
    }
}
