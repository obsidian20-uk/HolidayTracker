using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HolidayTracker.Models
{
    public class Holiday : IEntity, INotifyPropertyChanged
    {
        private DateTime _start;

        [Key]
        public int ID { get; set; }

        public Guid WholeHolidayGuid { get; set; }

        public bool Split { get; set; }

        public string Description { get; set; }

        public DateTime Start
        {
            get
            {
                return _start;
            }
            set {
                _start = value;
                OnPropertyChanged();
            }
        }

        public DateTime End { get; set; }

        public int NumDays { get; set; }

        public string HolidayDates {
            get
            {
                return $"{Start.Date.DayOfWeek} {Start.Date.ToShortDateString()} to {End.Date.DayOfWeek} {End.Date.ToShortDateString()} ({NumDays} days)";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
