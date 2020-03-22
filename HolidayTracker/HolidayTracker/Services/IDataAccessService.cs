using HolidayTracker.Models;
using HolidayTracker.Modules;
using System;
using System.Collections.Generic;

namespace HolidayTracker.Services
{
    public interface IDataAccessService
    {
        event EventHandler DataUpdate;

        void CreateHoliday(Holiday holiday);
        void CreateHolidayPeriod();
        void DeleteHoliday(Holiday holiday);
        IEnumerable<Holiday> GetAll();
        HolidayPeriod GetHolidayPeriod(int id);

        HolidayPeriod GetHolidayPeriod(DateTime date);
        IEnumerable<Holiday> GetHolidaysInPeriod(int holidayPeriodID);
        Setting GetSetting(string Key);
        void UpdateHoliday(Holiday holiday);
        void UpsertSetting(string Key, string Value);

        void CreateTestData();
    }
}