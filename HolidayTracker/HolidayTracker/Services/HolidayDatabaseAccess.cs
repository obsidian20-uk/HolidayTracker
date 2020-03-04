using HolidayTracker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayTracker.Services
{
    public class HolidayDatabaseAccess : IDataAccess<Holiday>
    {
        HolidayDatabaseContext _holidayDatabaseContext;

        public HolidayDatabaseAccess(HolidayDatabaseContext holidayDatabaseContext)
        {
            _holidayDatabaseContext = holidayDatabaseContext;
            if (!_holidayDatabaseContext.Holidays.Local.Any())
            {
                Upsert(new Holiday()
                {
                    Description = "Test",
                    Start = DateTime.Now,
                    End = DateTime.Now,
                    HolidayAllowancePeriod = 1
                });
            }
        }

        public void Delete(Holiday item)
        {
            _holidayDatabaseContext.Holidays.Remove(item);
            _holidayDatabaseContext.SaveChanges();
        }

        public Holiday Get(int ID)
        {
            return _holidayDatabaseContext.Holidays.FirstOrDefault(h => h.ID == ID);
        }

        public ObservableCollection<Holiday> GetAll()
        {
            return _holidayDatabaseContext.Holidays.Local.ToObservableCollection();
        }


        public ObservableCollection<Holiday> GetAllInPeriod(HolidayPeriod holidayAllowance)
        {
            _holidayDatabaseContext.Holidays.Where(h => h.HolidayAllowancePeriod == holidayAllowance.ID).Load();
            return _holidayDatabaseContext.Holidays.Local.ToObservableCollection();
        }


        public Holiday GetByDate(DateTime dateTime)
        {
            return _holidayDatabaseContext.Holidays.FirstOrDefault(h => h.Start <= dateTime && h.End >= dateTime);
        }

        public void Upsert(Holiday item)
        {
            if (_holidayDatabaseContext.Holidays.Any(h => h.ID == item.ID))
            {
                var currentitem = _holidayDatabaseContext.Holidays.FirstOrDefault(h => h.ID == item.ID);
                currentitem = item;
            }
            else
            {
                _holidayDatabaseContext.Holidays.Add(item);
            }
            _holidayDatabaseContext.SaveChanges();
        }
    }
}