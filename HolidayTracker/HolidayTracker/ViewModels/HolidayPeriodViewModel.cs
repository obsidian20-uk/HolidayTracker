using Android.OS;
using HolidayTracker.Models;
using HolidayTracker.Services;
using HolidayTracker.Views;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace HolidayTracker.ViewModels
{
    public class HolidayPeriodViewModel : ContentPage, IViewModel
    {
        public IDataAccessService _DataAccessService { get; set; }

        public Command<HolidayPeriod> cmdChangePeriod { get; set; }

        public Command<HolidayPeriod> cmdEditCurrentPeriod { get; set; }

        public Command<HolidayPeriod> cmdCreateNewPeriod { get; set; }

        public List<HolidayPeriod> holidayPeriods { get; set; }

        public HolidayPeriod CurrentHolidayPeriod { get; set; }

        public HolidayPeriod newHolidayPeriod { get; set; } = new HolidayPeriod();


        public HolidayPeriodViewModel(IDataAccessService DataAccessService)
        {
            _DataAccessService = DataAccessService;
            CurrentHolidayPeriod = Global.CurrentHolidayPeriod;
            holidayPeriods = _DataAccessService.GetHolidayPeriods();
            cmdChangePeriod = new Command<HolidayPeriod>(hp => ChangeHolidayPeriod());
            //cmdEditCurrentPeriod = new Command<HolidayPeriod>(hp => EditCurrentPeriod());

            cmdCreateNewPeriod = new Command<HolidayPeriod>(hp => CreatePeriod(newHolidayPeriod));
            Title = "Not in Work - Holiday Periods";

        }

        public void ChangeHolidayPeriod()
        {
            Global.CurrentHolidayPeriod = CurrentHolidayPeriod;
            App.Current.MainPage = new MainView(new HolidaysView());
        }

        public void CreatePeriod(HolidayPeriod newHolidayPeriod)
        {
            if (_DataAccessService.CheckForHolidayPeriodOverlap(newHolidayPeriod))
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Holiday period overlap", "Holiday period overlaps with existing one", "OK");
                });
            }
            else if (newHolidayPeriod.NumHolidays <= 0)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Number of Days missing", "You need to provide a number of days for holiday period", "OK");
                });
            }
            else
            {
                var webData = new WebData(_DataAccessService);
                webData.UpdatePublicHolidays(true);
                _DataAccessService.CreateHolidayPeriod(newHolidayPeriod);
                App.Current.MainPage = new MainView(new HolidaysView());
            }
        }
    }
}
