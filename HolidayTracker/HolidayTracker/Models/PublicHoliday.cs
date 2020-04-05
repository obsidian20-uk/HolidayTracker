using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HolidayTracker.Models
{
    public class PublicHoliday
    {
        [Key]
        public int ID { get; set; }
        public string Description { get; set; }

        public DateTime Date { get; set; }
    }
}
