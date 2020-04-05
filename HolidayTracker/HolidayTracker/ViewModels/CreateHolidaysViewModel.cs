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
        }
        public Holiday newHoliday { get; set; } = new Holiday();

        public void CreateHoliday(Holiday holiday)
        {
            _DataAccessService.CreateHoliday(holiday);
            //var mdp = Application.Current.MainPage as MasterDetailPage;
            //mdp.Detail..PopAsync();

            App.Current.MainPage = new MainView(new HolidaysView());
        }
    }
}
