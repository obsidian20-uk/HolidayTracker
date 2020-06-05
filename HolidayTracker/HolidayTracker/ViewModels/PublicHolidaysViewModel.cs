using HolidayTracker.Models;
using HolidayTracker.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace HolidayTracker.ViewModels
{
    public class PublicHolidaysViewModel : ContentPage, IViewModel
    {
        IDataAccessService DataAccessService { get; set; }
        public PublicHolidaysViewModel(IDataAccessService dataAccessService)
        {
            Title = "Not in Work - Public Holidays";
            DataAccessService = dataAccessService;
            PublicHolidays = DataAccessService.GetPublicHolidays(DateTime.MinValue, DateTime.MaxValue).OrderBy(H => H.Date).ToList();
        }

        public List<PublicHoliday> PublicHolidays { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
