﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HolidayTracker.Models
{
    public class HolidayAllowance: IEntity
    {
        [Key]
        public int ID { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
        public int NumDays { get; set; }

    }
}