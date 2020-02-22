using HolidayTracker.Models;
using HolidayTracker.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HolidayTracker.Services
{
    public class HolidayAllowanceDatabaseAccess : IDataAccess<HolidayAllowance>
    {
        HolidayDatabaseContext _HolidayDatabaseContext;

        public HolidayAllowanceDatabaseAccess(HolidayDatabaseContext HolidayDatabaseContext)
        {
            _HolidayDatabaseContext = HolidayDatabaseContext;
        }


        public async Task Delete(HolidayAllowance item)
        {
            _HolidayDatabaseContext.HolidayAllowances.Remove(item);
        }

        public async Task<HolidayAllowance> Get(int ID)
        {
            return await _HolidayDatabaseContext.HolidayAllowances.FirstOrDefaultAsync(h => h.ID == ID);
        }

        public async Task<IEnumerable<HolidayAllowance>> GetAll()
        {
            return await _HolidayDatabaseContext.HolidayAllowances.ToListAsync();
        }

        public async Task Upsert(Task<HolidayAllowance> item)
        {
            if (await _HolidayDatabaseContext.HolidayAllowances.AnyAsync(h => h.ID == item.Result.ID))
            {
                var currentitem = _HolidayDatabaseContext.HolidayAllowances.FirstOrDefaultAsync(h => h.ID == item.Result.ID);
                currentitem = item;
            }
            else
            {
                await _HolidayDatabaseContext.HolidayAllowances.AddAsync(item.Result);
            }
        }
    }
}
