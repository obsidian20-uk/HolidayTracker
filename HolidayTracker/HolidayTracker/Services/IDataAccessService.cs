using HolidayTracker.Models;
using HolidayTracker.Modules;
using System;
using System.Collections.Generic;

namespace HolidayTracker.Services
{
    public interface IDataAccessService
    {
        event DataAccessEventHandler DataUpdate;

        void CreateHoliday(Holiday holiday);
        void CreateHolidayPeriod();
        void DeleteHoliday(Holiday holiday);
        IEnumerable<Holiday> GetAll();
        HolidayPeriod GetHolidayPeriod(int id);
        IEnumerable<Holiday> GetHolidaysInPeriod(int holidayPeriodID);
        Setting GetSetting(string Key);
        void UpdateHoliday(Holiday holiday);
        void UpsertSetting(string Key, string Value);
    }
}