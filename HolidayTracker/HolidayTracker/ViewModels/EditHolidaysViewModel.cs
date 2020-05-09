using HolidayTracker.Models;
using HolidayTracker.Services;
using HolidayTracker.Views;
using Ninject;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HolidayTracker.ViewModels
{
    public class EditHolidaysViewModel : ContentPage, IViewModel
    {
        [Inject]
        public IDataAccessService _DataAccessService { get; set; }

        public Command<Holiday> EditHolidayCommand { get; set; }

        public Holiday Holiday { get; set; } = new Holiday();

        public EditHolidaysViewModel(Holiday holiday)
        {
            Holiday = holiday;
            EditHolidayCommand = new Command<Holiday>(h => UpdateHoliday(Holiday));
        }

        public void UpdateHoliday(Holiday holiday)
        {
            _DataAccessService.UpdateHoliday(holiday);
            //var mdp = Application.Current.MainPage as MasterDetailPage;
            //mdp.Detail..PopAsync();

            App.Current.MainPage = new MainView(new HolidaysView());
        }
    }
}
