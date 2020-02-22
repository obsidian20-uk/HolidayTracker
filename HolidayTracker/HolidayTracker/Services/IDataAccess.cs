using HolidayTracker.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HolidayTracker.Services
{
    public interface IDataAccess<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Get(int ID);
        Task Upsert(Task<TEntity> item);
        Task Delete(TEntity item);
    }
}
