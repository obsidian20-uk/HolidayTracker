using HolidayTracker.Models;
using HolidayTracker.Services;
using HolidayTracker.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace HolidayTracker.ViewModels
{
    public class SettingsViewModel : ContentPage, IViewModel
    {
        public IDataAccessService _DataAccessService { get; set; }

        public bool WorkWeekends { get; set; }

        public bool WorkPublicHolidays { get; set; }

        public Command<HolidayPeriod> cmdUpdateSettings { get; set; }

        public Command<HolidayPeriod> cmdUpdatePublicHolidays { get; set; }

        public KeyValuePair<string, string> Country { get; set; }

        public List<KeyValuePair<string, string>> Countries { get; set; } = Global.Countries.ToList();

        public SettingsViewModel(IDataAccessService DataAccessService)
        {
            _DataAccessService = DataAccessService;
            cmdUpdateSettings = new Command<HolidayPeriod>(h => UpdateSettings());
            cmdUpdatePublicHolidays = new Command<HolidayPeriod>(h => UpdatePublicHolidays());
            WorkWeekends = bool.Parse(_DataAccessService.GetSetting("WorkWeekends"));
            WorkPublicHolidays = bool.Parse(_DataAccessService.GetSetting("WorkPublicHolidays"));
            Title = "Holiday Tracker - Settings";
        }

        private void UpdatePublicHolidays()
        {
            _DataAccessService.UpsertSetting("Country", Country.Key);
            var webData = new WebData(_DataAccessService);
            webData.UpdatePublicHolidays(true);
            App.Current.MainPage = new MainView(new HolidaysView());
        }

        public void UpdateSettings()
        {
            _DataAccessService.UpsertSetting("WorkWeekends", WorkWeekends.ToString());
            _DataAccessService.UpsertSetting("WorkPublicHolidays", WorkPublicHolidays.ToString());
            App.Current.MainPage = new MainView(new HolidaysView());
        }
    }
}
