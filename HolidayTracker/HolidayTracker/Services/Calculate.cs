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
                DaysCount += holiday.NumDays;
            }

            return DaysCount;
        }

        public static int PercentageUsed(IEnumerable<Holiday> holidays, HolidayPeriod holidayPeriod)
        {
            return holidayPeriod.NumHolidays / DaysUsed(holidays);
        }

        public static int DaysRemaining(IEnumerable<Holiday> holidays, HolidayPeriod holidayPeriod)
        {
            return holidayPeriod.NumHolidays - DaysUsed(holidays);
        }

        public static int DaysSinceLastHoliday(IEnumerable<Holiday> holidays)
        {
            return -1 * holidays.OrderByDescending(h => h.End).FirstOrDefault().End.Subtract(DateTime.Now).Days;
        }

        public static int DaysToNextHoliday(IEnumerable<Holiday> holidays)
        {
            return holidays.OrderBy(h => h.Start).FirstOrDefault(h => h.Start >= DateTime.Today).Start.Subtract(DateTime.Now).Days;
        }

        public static int NumDaysInHoliday(DateTime start, DateTime end, List<PublicHoliday> publicHolidays, bool WorkWeekends = false, bool WorkPublicHolidays = false)
        {
            List<DateTime> dates = new List<DateTime>();
            int NumWeekendDays = 0;
            int NumPublicHolidays = 0;

            for (DateTime i = start; i <= end; i = i.AddDays(1))
            {
                dates.Add(i);
            }

            int NumDays = dates.Count;

            if (!WorkWeekends)
            {
                NumWeekendDays = dates.Count(d => d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday);
                NumDays -= NumWeekendDays;
            }

            if (!WorkPublicHolidays)
            {
                NumPublicHolidays = publicHolidays.Count(ph => ph.Date >= start && ph.Date <= end);
                NumDays -= NumPublicHolidays;
            }

            return NumDays;
        }
    }
}
