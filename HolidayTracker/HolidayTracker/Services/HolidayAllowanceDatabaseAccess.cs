using HolidayTracker.Models;
using HolidayTracker.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayTracker.Services
{
    public class HolidayAllowanceDatabaseAccess : IDataAccess<HolidayAllowance>
    {
        HolidayDatabaseContext _holidayDatabaseContext;

        public HolidayAllowanceDatabaseAccess(HolidayDatabaseContext HolidayDatabaseContext)
        {
            _holidayDatabaseContext = HolidayDatabaseContext;
        }


        public void Delete(HolidayAllowance item)
        {
            _holidayDatabaseContext.HolidayAllowances.Remove(item);
        }

        public HolidayAllowance Get(int ID)
        {
            return _holidayDatabaseContext.HolidayAllowances.FirstOrDefault(h => h.ID == ID);
        }

        public IEnumerable<HolidayAllowance> GetAll()
        {
            return _holidayDatabaseContext.HolidayAllowances.ToList();
        }

        public HolidayAllowance GetByDate(DateTime dateTime)
        {
            return _holidayDatabaseContext.HolidayAllowances.FirstOrDefault(h => h.Start <= dateTime && h.End >= dateTime);
        }

        public void Upsert(HolidayAllowance item)
        {
            if (_holidayDatabaseContext.HolidayAllowances.Any(h => h.ID == item.ID))
            {
                var currentitem = _holidayDatabaseContext.HolidayAllowances.FirstOrDefault(h => h.ID == item.ID);
                currentitem = item;
            }
            else
            {
                _holidayDatabaseContext.HolidayAllowances.Add(item);
            }
        }


    }
}
