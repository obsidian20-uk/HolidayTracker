using HolidayTracker.Models;
using HolidayTracker.Services;
using HolidayTracker.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HolidayTracker.ViewModels
{
    public class CreateHolidaysViewModel : ContentPage, IViewModel
    {
        public IDataAccessService _DataAccessService { get; set; }

        public Command<Holiday> CreateHolidayCommand { get; set; }

        public CreateHolidaysViewModel(IDataAccessService DataAccessService)
        {
            _DataAccessService = DataAccessService;
            CreateHolidayCommand = new Command<Holiday>(h => CreateHoliday(newHoliday));
            Title = "Not in Work - Create Holiday";

        }
        public Holiday newHoliday { get; set; } = new Holiday();

        public void CreateHoliday(Holiday holiday)
        {
            if (!_DataAccessService.CheckHolidayPeriodExists(holiday.End) || !_DataAccessService.CheckHolidayPeriodExists(holiday.Start))
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Holiday period does not exist", "You cannot create holiday which is in holiday period you haven't created yet", "OK");
                });
            }
            else
            {
                _DataAccessService.CreateHoliday(holiday);
                App.Current.MainPage = new MainView(new HolidaysView());
            }
        }
    }
}
