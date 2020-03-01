using HolidayTracker.Models;
using HolidayTracker.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace HolidayTracker.ViewModels
{
    class MainViewModel : Page, IViewModel
    {
        public ObservableCollection<Holiday> Holidays { get; set; }

        IDataAccess<Holiday> holidayDatabaseAccess;

        public int NumDaysUsed { get; set; }

        public MainViewModel()
        {           
            holidayDatabaseAccess = Global.kernel.Get<IDataAccess<Holiday>>();
            Holidays = new ObservableCollection<Holiday>(holidayDatabaseAccess.GetAll().ToList());
            var calcs = new Calculations();
            NumDaysUsed = calcs.CalculateDaysUsed(Holidays.ToList());
            Title = "Test";
        }

        public bool IsBusy { get; set ; }
        public string Title { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
