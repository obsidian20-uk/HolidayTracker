﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace HolidayTracker.Models
{
    public class Holiday : IEntity
    {
        [Key]
        public int ID { get; set; }

        public int HolidayAllowancePeriod { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public static implicit operator Task<object>(Holiday v)
        {
            throw new NotImplementedException();
        }
    }
}
