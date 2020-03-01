using HolidayTracker.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HolidayTracker.Services
{
    public interface IDataAccess<IEntity>
    {
        IEnumerable<IEntity> GetAll();
        IEntity Get(int ID);
        IEntity GetByDate(DateTime dateTime);
        void Upsert(IEntity item);
        void Delete(IEntity item);
    }
}
