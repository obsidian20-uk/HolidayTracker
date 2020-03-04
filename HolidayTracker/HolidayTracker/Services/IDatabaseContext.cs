using HolidayTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HolidayTracker.Services
{
    public interface IDatabaseContext
    {
        DbSet<HolidayPeriod> HolidayPeriods { get; set; }
        DbSet<Holiday> Holidays { get; set; }

        void Initialise();
    }
}