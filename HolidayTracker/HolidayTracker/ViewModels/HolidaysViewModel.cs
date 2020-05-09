using HolidayTracker.Models;
using HolidayTracker.Modules;
using HolidayTracker.Services;
using HolidayTracker.Views;
using Microsoft.EntityFrameworkCore;
using Ninject;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace HolidayTracker.ViewModels
{
    class HolidaysViewModel : ContentPage, IViewModel, INotifyPropertyChanged
    {
        private ObservableCollection<Holiday> holidays;
        private int numDaysUsed;

        public string currentHolidayPeriodText { get; set; }

        public ObservableCollection<Holiday> Holidays
        {
            get => holidays;
            set
            {
                holidays = value;
                OnPropertyChanged();
            }
        }
        public HolidayPeriod CurrentHolidayPeriod { get; set; }

        public IDataAccessService _DataAccessService { get; set; }

        public int NumDaysUsed
        {
            get => numDaysUsed;
            set
            {
                numDaysUsed = value;
                OnPropertyChanged();
            }
        }

        public Command<Holiday> DeleteHoliday { get; set; }

        public Command<Holiday> EditHoliday { get; set; }

        public HolidaysViewModel(IDataAccessService DataAccessService)
        {
            Title = "Holiday Tracker";
            _DataAccessService = DataAccessService;
            DeleteHoliday = new Command<Holiday>(holiday => _DataAccessService.DeleteHoliday(holiday));
            EditHoliday = new Command<Holiday>(holiday => UpdateHoliday(holiday));
            _DataAccessService.Setup();
            CurrentHolidayPeriod = _DataAccessService.GetHolidayPeriod(DateTime.Now);
            _DataAccessService.DataUpdate += Data_Changed;
            Holidays = new ObservableCollection<Holiday>(_DataAccessService.GetHolidaysInPeriod(CurrentHolidayPeriod.ID));
            UpdateScreen();
            currentHolidayPeriodText = CurrentHolidayPeriod.ToString();
        }

        private async void UpdateHoliday(Holiday holiday)
        {
            await PopupNavigation.PushAsync(new EditHolidaysView(holiday));
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

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
