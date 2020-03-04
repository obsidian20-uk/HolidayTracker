using HolidayTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace HolidayTracker.Services
{
    public class Calculate
    {
        public static int DaysUsed(IEnumerable<Holiday> holidays)
        {
            var DaysCount = 0;
            foreach (var holiday in holidays)
            {
                DaysCount += holiday.End.Subtract(holiday.Start).Duration().Days + 1;
            }

            return DaysCount;
        }

        public static int PercentageUsed(IEnumerable<Holiday> holidays, HolidayPeriod holidayPeriod)
        {
            return holidayPeriod.NumDays / DaysUsed(holidays);
        }

        public static int DaysRemaining(IEnumerable<Holiday> holidays, HolidayPeriod holidayPeriod)
        {
            return holidayPeriod.NumDays - DaysUsed(holidays);
        }

        public static int DaysSinceLastHoliday(IEnumerable<Holiday> holidays)
        {
            return -1 * holidays.OrderByDescending(h => h.End).FirstOrDefault().End.Subtract(DateTime.Now).Days;
        }

        public static int DaysToNextHoliday(IEnumerable<Holiday> holidays)
        {
            return holidays.OrderBy(h => h.Start).FirstOrDefault(h => h.Start > DateTime.Now).Start.Subtract(DateTime.Now).Days;
        }
    }
}
