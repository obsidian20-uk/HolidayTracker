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
                    new MainMenuItem { Id = 1, Title = "Create New Holiday", TargetType=typeof(CreateHolidaysView)},
                    new MainMenuItem { Id = 2, Title = "Change Holiday Period" },
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
