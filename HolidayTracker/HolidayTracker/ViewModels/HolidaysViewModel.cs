using HolidayTracker.Models;
using HolidayTracker.Modules;
using HolidayTracker.Services;
using HolidayTracker.Views;
using Microcharts;
using Microsoft.EntityFrameworkCore;
using Ninject;
using SkiaSharp;
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

        public WebData webData;
        private int daysToNext;

        public Command<Holiday> DeleteHoliday { get; set; }

        public Command<Holiday> EditHoliday { get; set; }
        public Command<Holiday> NewHoliday { get; set; }

        public int DaysToNext
        {
            get => daysToNext; set
            {
                daysToNext = value;
                OnPropertyChanged();
            }
        }

        public DonutChart chartData { get; set; } = new DonutChart();

        public bool PeriodCreated { get; set; }

        public bool PeriodNotCreated { get { return !PeriodCreated; } }

        public HolidaysViewModel(IDataAccessService DataAccessService)
        {
            Title = "Not in Work";
            _DataAccessService = DataAccessService;
            DeleteHoliday = new Command<Holiday>(holiday => _DataAccessService.DeleteHoliday(holiday));
            NewHoliday = new Command<Holiday>(holiday => NewHol());
            EditHoliday = new Command<Holiday>(holiday => EditHol(holiday));
            _DataAccessService.Setup();
            CurrentHolidayPeriod = _DataAccessService.GetHolidayPeriod(DateTime.Now);
            if (CurrentHolidayPeriod != null)
            {
                Holidays = new ObservableCollection<Holiday>(_DataAccessService.GetHolidaysInPeriod(CurrentHolidayPeriod.ID));
                UpdateScreen();
            }

        }

        private void Data_Changed(object sender, EventArgs e)
        {
            UpdateScreen();
        }

        private void EditHol(Holiday holiday)
        {
            ((App.Current.MainPage as MasterDetailPage).Detail as NavigationPage).Navigation.PushAsync(new EditHolidaysView());
            MessagingCenter.Send(holiday, "EditHol");
        }

        private void NewHol()
        {
            ((App.Current.MainPage as MasterDetailPage).Detail as NavigationPage).Navigation.PushAsync(new CreateHolidaysView());
        }


        private void UpdateScreen()
        {
            Holidays = new ObservableCollection<Holiday>(_DataAccessService.GetHolidaysInPeriod(CurrentHolidayPeriod.ID).OrderBy(h => h.Start));
            NumDaysUsed = Calculate.DaysUsed(Holidays.ToList());
            var DaysLeft = CurrentHolidayPeriod.NumHolidays - NumDaysUsed;
            chartData.BackgroundColor = SKColor.Empty;
            chartData.GraphPosition = Microcharts.Layouts.GraphPosition.AutoFill;
            chartData.LabelMode = Microcharts.Layouts.LabelMode.RightOnly;
            chartData.LabelTextSize = 40;
            chartData.Entries = new[]
            {
                new Microcharts.Entry(NumDaysUsed)
                {
                    Label = "Days Booked",
                    ValueLabel = NumDaysUsed.ToString(),
                    Color = SKColor.Parse("#006400"),
                    TextColor = SKColor.Parse("#000000")
                },
                new Microcharts.Entry(DaysLeft)
                {
                    Label = "Days Left",
                    ValueLabel = DaysLeft.ToString(),
                    Color = SKColor.Parse("#FF0000"),
                    TextColor = SKColor.Parse("#000000")
                }
            };
            currentHolidayPeriodText = CurrentHolidayPeriod.ToString();
            _DataAccessService.DataUpdate += Data_Changed;
            var FutureHols = _DataAccessService.GetFutureHolidays();
            if (FutureHols.Any())
            {
                DaysToNext = Calculate.DaysToNextHoliday(FutureHols);
            }
            else
            {
                DaysToNext = 999;
            }
            webData = new WebData(_DataAccessService);
            webData.UpdatePublicHolidays();
            PeriodCreated = (CurrentHolidayPeriod != null);
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
