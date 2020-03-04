using HolidayTracker.Models;
using HolidayTracker.Services;
using Microsoft.EntityFrameworkCore;
using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace HolidayTracker.ViewModels
{
    class MainViewModel : Page, IViewModel
    {

        public ObservableCollection<Holiday> Holidays { get; set; }
        public HolidayPeriod CurrentHolidayAllowance { get; set; }

        IDataAccess<Holiday> holidayDatabaseAccess;

        IDataAccess<HolidayPeriod> holidayAllowanceDatabaseAccess;

        public int NumDaysUsed { get; set; }

        public Command<Holiday> DeleteHoliday { get; set; }

        public Command<Holiday> EditHoliday { get; set; }

        public MainViewModel()
        {
            holidayDatabaseAccess = Global.kernel.Get<IDataAccess<Holiday>>();
            holidayAllowanceDatabaseAccess = Global.kernel.Get<IDataAccess<HolidayPeriod>>();
            
            CurrentHolidayAllowance = holidayAllowanceDatabaseAccess.GetByDate(DateTime.Now);
            Holidays = holidayDatabaseAccess.GetAllInPeriod(CurrentHolidayAllowance);
            UpdateData();
            Holidays.CollectionChanged += Holidays_Changed;
            Title = "Holiday Tracker";
            DeleteHoliday = new Command<Holiday>(holiday => holidayDatabaseAccess.Delete(holiday));
        }

        private void Holidays_Changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            NumDaysUsed = Calculate.DaysUsed(Holidays.ToList());
        }

        public bool IsBusy { get; set; }
        public string Title { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
