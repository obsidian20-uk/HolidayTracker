using HolidayTracker.Models;
using HolidayTracker.Services;
using System.ComponentModel;

namespace HolidayTracker.ViewModels
{
    public interface IViewModel
    {
        bool IsBusy { get; set; }
        string Title { get; set; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}