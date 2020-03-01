using HolidayTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HolidayTracker.Services
{
    public class Calculations
    {
        public int CalculateDaysUsed(List<Holiday> holidays)
        {
            var DaysCount = 0;
            foreach (var holiday in holidays)
            {
                DaysCount += holiday.End.Subtract(holiday.Start).Duration().Days + 1;
            }

            return DaysCount;
        }

        public int CalculateDaysRemaining(List<Holiday> holidays, HolidayAllowance holidayAllowance)
        {
            return holidayAllowance.NumDays - CalculateDaysUsed(holidays);
        }
    }
}
