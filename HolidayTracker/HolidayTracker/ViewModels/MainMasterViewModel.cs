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
                    new MainMenuItem { Id = 0, Title = "Page 1" },
                    new MainMenuItem { Id = 1, Title = "Page 2" },
                    new MainMenuItem { Id = 2, Title = "Page 3" },
                    new MainMenuItem { Id = 3, Title = "Page 4" },
                    new MainMenuItem { Id = 4, Title = "Page 5" },
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
