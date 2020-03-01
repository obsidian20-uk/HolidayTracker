using HolidayTracker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        }

        public void Delete(Holiday item)
        {
            _holidayDatabaseContext.Holidays.Remove(item);
        }

        public Holiday Get(int ID)
        {
            return _holidayDatabaseContext.Holidays.FirstOrDefault(h => h.ID == ID);
        }

        public IEnumerable<Holiday> GetAll()
        {
            return _holidayDatabaseContext.Holidays.ToList();
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