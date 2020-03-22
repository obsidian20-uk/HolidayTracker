using HolidayTracker.Models;
using HolidayTracker.Modules;
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
    class HolidaysViewModel : ContentPage, IViewModel
    {

        public ObservableCollection<Holiday> Holidays { get; set; }
        public HolidayPeriod CurrentHolidayPeriod { get; set; }

        public IDataAccessService _DataAccessService { get; set; }

        public int NumDaysUsed { get; set; }

        public Command<Holiday> DeleteHoliday { get; set; }

        public Command<Holiday> EditHoliday { get; set; }

        public HolidaysViewModel(IDataAccessService DataAccessService)
        {
            Title = "Holiday Tracker";
            DeleteHoliday = new Command<Holiday>(holiday => _DataAccessService.DeleteHoliday(holiday));
            _DataAccessService = DataAccessService;
            CurrentHolidayPeriod = _DataAccessService.GetHolidayPeriod(DateTime.Now);
            _DataAccessService.DataUpdate += Data_Changed;
            //_DataAccessService.CreateTestData();
            UpdateScreen();
        }

        private void Data_Changed(object sender, EventArgs e)
        {
            UpdateScreen();
        }

        private void UpdateScreen()
        {
            Holidays = new ObservableCollection<Holiday>(_DataAccessService.GetHolidaysInPeriod(CurrentHolidayPeriod.ID));
            NumDaysUsed = Calculate.DaysUsed(CurrentHolidayPeriod.Holidays.ToList());
        }

        public bool IsBusy { get; set; }
        public string Title { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
