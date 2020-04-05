using HolidayTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HolidayTracker.Services
{
    public interface IDatabaseContext
    {
        DbSet<HolidayPeriod> HolidayPeriods { get; set; }
        DbSet<Holiday> Holidays { get; set; }

        DbSet<Setting> Settings { get; set; }

        DbSet<PublicHoliday> PublicHolidays { get; set; }

        void Save();

        void Initialise();

        /// <summary>
        /// Only used for testing. Will wipe whole database
        /// </summary>
        void Reset();
    }
}