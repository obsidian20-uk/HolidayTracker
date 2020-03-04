using HolidayTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HolidayTracker.Services
{
    public interface IDataAccess<IEntity>
    {
        ObservableCollection<IEntity> GetAllInPeriod(HolidayPeriod holidayAllowance);
        ObservableCollection<IEntity> GetAll();
        IEntity Get(int ID);
        IEntity GetByDate(DateTime dateTime);
        void Upsert(IEntity item);
        void Delete(IEntity item);
    }
}
