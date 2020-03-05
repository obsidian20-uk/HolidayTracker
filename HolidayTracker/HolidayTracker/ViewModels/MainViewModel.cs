using HolidayTracker.Models;
using HolidayTracker.Services;
using Microsoft.EntityFrameworkCore;
using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace HolidayTracker.ViewModels
{
    class MainViewModel : Page, IViewModel
    {

        public ObservableCollection<Holiday> Holidays { get; set; }
        public HolidayPeriod CurrentHolidayPeriod { get; set; }

        IDataAccessService _DataAccessService;

        public int NumDaysUsed { get; set; }

        public Command<Holiday> DeleteHoliday { get; set; }

        public Command<Holiday> EditHoliday { get; set; }

        public MainViewModel()
        {
            Title = "Holiday Tracker";
            DeleteHoliday = new Command<Holiday>(holiday => _DataAccessService.DeleteHoliday(holiday));
            _DataAccessService.DataUpdate += Data_Changed;
            UpdateScreen();
        }

        private void Data_Changed(object sender, EventArgs e)
        {
            UpdateScreen();
        }

        private void UpdateScreen()
        {
            NumDaysUsed = Calculate.DaysUsed(CurrentHolidayPeriod.Holidays.ToList());
            Holidays = new ObservableCollection<Holiday>(_DataAccessService.GetHolidaysInPeriod(CurrentHolidayPeriod.ID));
        }

        public bool IsBusy { get; set; }
        public string Title { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
