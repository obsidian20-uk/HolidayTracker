using System;

namespace HolidayTracker.Models
{
    public interface IEntity
    {
        DateTime End { get; set; }
        int ID { get; set; }
        DateTime Start { get; set; }
    }
}