using HolidayTracker.Models;
using HolidayTracker.Services;
using HolidayTracker.Views;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace HolidayTracker.ViewModels
{
    public class EditHolidaysViewModel : ContentPage, IViewModel, INotifyPropertyChanged
    {
        [Inject]
        public IDataAccessService _DataAccessService { get; set; }

        public Command<Holiday> EditHolidayCommand { get; set; }

        private Holiday _holiday;

        public Holiday Holiday
        {
            get
            {
                return _holiday;
            }
            set
            {
                _holiday = value;
                OnPropertyChanged();
            }
        }

        public EditHolidaysViewModel()
        {
            MessagingCenter.Subscribe<Holiday>(this, "EditHol", (holiday) =>
            {
                Holiday = holiday;
            });
            EditHolidayCommand = new Command<Holiday>(h => UpdateHoliday(Holiday));
            Title = "Not in Work - Edit Holiday";

        }

        public void UpdateHoliday(Holiday holiday)
        {
            _DataAccessService.UpdateHoliday(holiday);
            //var mdp = Application.Current.MainPage as MasterDetailPage;
            //mdp.Detail..PopAsync();

            App.Current.MainPage = new MainView(new HolidaysView());
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
