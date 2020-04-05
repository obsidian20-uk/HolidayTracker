using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HolidayTracker.Models
{
    public class HolidayPeriod: IEntity
    {
        [Key]
        public int ID { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
        public int NumHolidays { get; set; }

        public List<Holiday> Holidays { get; set; } = new List<Holiday>();

        public string ToString()
        {
            if (End.Subtract(Start).Days > 360)
            {
                if (Start.Year == End.Year)
                {
                    return $"Holiday Year: {Start.Year}";
                }
                return $"Holiday Year: {Start.Year} / {End.Year}";
            }
            else
            {
                return $"Holiday Year: {Start.Date} to {End.Date}";
            }
        }
    }
}
