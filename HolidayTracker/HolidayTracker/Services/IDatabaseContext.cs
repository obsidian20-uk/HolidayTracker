using HolidayTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HolidayTracker.Services
{
    public interface IDatabaseContext
    {
        DbSet<HolidayAllowance> HolidayAllowances { get; set; }
        DbSet<Holiday> Holidays { get; set; }

        void Initialise();
    }
}