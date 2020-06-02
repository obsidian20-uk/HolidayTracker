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

        void CreateHolidayPeriod(HolidayPeriod newHolidayPeriod);
        void DeleteHoliday(Holiday holiday);
        IEnumerable<Holiday> GetAll();
        HolidayPeriod GetHolidayPeriod(int id);

        HolidayPeriod GetHolidayPeriod(DateTime date);
        IEnumerable<Holiday> GetHolidaysInPeriod(int holidayPeriodID);
        string GetSetting(string Key);
        void UpdateHoliday(Holiday holiday);
        void UpsertSetting(string Key, string Value);

        List<HolidayPeriod> GetHolidayPeriods();

        void AddPublicHoliday(PublicHoliday publicHoliday);

        IEnumerable<PublicHoliday> GetPublicHolidays(DateTime start, DateTime end);

        bool CheckForHolidayPeriodOverlap(HolidayPeriod possibleHolidayPeriod);


        void CreateTestData();

        void Setup();
    }
}