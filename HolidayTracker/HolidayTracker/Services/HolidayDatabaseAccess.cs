using HolidayTracker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task Delete(Holiday item)
        {
            _holidayDatabaseContext.Holidays.Remove(item);
        }

        public async Task<Holiday> Get(int ID)
        {
            return await _holidayDatabaseContext.Holidays.FirstOrDefaultAsync(h => h.ID == ID);
        }

        public async Task<IEnumerable<Holiday>> GetAll()
        {
            return await _holidayDatabaseContext.Holidays.ToListAsync();
        }

        public async Task Upsert(Task<Holiday> item)
        {
            if (await _holidayDatabaseContext.Holidays.AnyAsync(h => h.ID == item.Result.ID))
            {
                var currentitem = _holidayDatabaseContext.Holidays.FirstOrDefaultAsync(h => h.ID == item.Result.ID);
                currentitem = item;
            }
            else
            {
                await _holidayDatabaseContext.Holidays.AddAsync(item.Result);
            }
        }
    }
}