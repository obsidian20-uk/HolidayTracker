using HolidayTracker.Models;
using HolidayTracker.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace HolidayTracker.ViewModels
{
    class MainViewModel : Page, IViewModel
    {
        public MainViewModel()
        {
            Title = "Test";
        }
        public bool IsBusy { get; set ; }
        public string Title { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
