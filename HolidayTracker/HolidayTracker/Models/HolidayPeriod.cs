using Java.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace HolidayTracker.Models
{
    public class HolidayPeriod : IEntity, INotifyPropertyChanged
    {
        private DateTime start;
        private DateTime end;

        [Key]
        public int ID { get; set; }

        public DateTime Start
        {
            get => start;
            set
            {
                start = value;
                End = start.AddDays(364);
            }
        }

        public DateTime End
        {
            get => end;
            set
            {
                end = value;
                OnPropertyChanged();
            }
        }

        public int NumHolidays { get; set; }

        public List<Holiday> Holidays { get; set; } = new List<Holiday>();

        public string Display
        {
            get
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
                    return $"Holiday Period: {Start.ToShortDateString()} to {End.ToShortDateString()}";
                }
            }
        }

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
                return $"Holiday Period: {Start.ToShortDateString()} to {End.ToShortDateString()}";
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
