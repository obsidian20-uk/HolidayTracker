using HolidayTracker.Models;
using HolidayTracker.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayTracker.Services
{
    public class HolidayPeriodDatabaseAccess : IDataAccess<HolidayPeriod>
    {
        HolidayDatabaseContext _holidayDatabaseContext;

        public HolidayPeriodDatabaseAccess(HolidayDatabaseContext HolidayDatabaseContext)
        {
            _holidayDatabaseContext = HolidayDatabaseContext;
            if (!_holidayDatabaseContext.Holidays.Local.Any())
            {
                Upsert(new HolidayPeriod()
                {
                    Start = new DateTime(DateTime.Now.Year, 1, 1),
                    End = new DateTime(DateTime.Now.Year, 12, 31),
                    NumDays = 25
                });
            }
        }


        public void Delete(HolidayPeriod item)
        {
            _holidayDatabaseContext.HolidayPeriods.Remove(item);
        }

        public HolidayPeriod Get(int ID)
        {
            return _holidayDatabaseContext.HolidayPeriods.FirstOrDefault(h => h.ID == ID);
        }

        public ObservableCollection<HolidayPeriod> GetAll()
        {
            _holidayDatabaseContext.HolidayPeriods.Load();
            return _holidayDatabaseContext.HolidayPeriods.Local.ToObservableCollection();
        }

        public ObservableCollection<HolidayPeriod> GetAllInPeriod(HolidayPeriod holidayAllowance)
        {
            throw new NotImplementedException();
        }

        public HolidayPeriod GetByDate(DateTime dateTime)
        {
            return _holidayDatabaseContext.HolidayPeriods.FirstOrDefault(h => h.Start <= dateTime && h.End >= dateTime);
        }

        public void Upsert(HolidayPeriod item)
        {
            if (_holidayDatabaseContext.HolidayPeriods.Any(h => h.ID == item.ID))
            {
                var currentitem = _holidayDatabaseContext.HolidayPeriods.FirstOrDefault(h => h.ID == item.ID);
                currentitem = item;
            }
            else
            {
                _holidayDatabaseContext.HolidayPeriods.Add(item);
            }
        }


    }
}
