using HolidayTracker.Models;
using HolidayTracker.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace HolidayTracker.ViewModels
{
    class MainMasterViewModel : ContentPage, IViewModel
    {
        public ObservableCollection<MainMenuItem> MenuItems { get; set; }

        public MainMasterViewModel()
        {
            MenuItems = new ObservableCollection<MainMenuItem>(new[]
            {
                    new MainMenuItem { Id = 0, Title = "View Holidays", TargetType=typeof(HolidaysView)},
                    new MainMenuItem { Id = 2, Title = "Holiday Periods", TargetType=typeof(HolidayPeriodView) },
                    new MainMenuItem { Id = 4, Title = "Public Holidays", TargetType=typeof(PublicHolidaysView) },
                    new MainMenuItem { Id = 4, Title = "Settings", TargetType=typeof(SettingsView) },
                });
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
