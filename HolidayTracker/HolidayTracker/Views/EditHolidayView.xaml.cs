using HolidayTracker.Models;
using HolidayTracker.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HolidayTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditHolidaysView : PopupPage
    {
        public EditHolidaysView(Holiday holiday)
        {
            InitializeComponent();
            this.BindingContext = new EditHolidaysViewModel(holiday);
        }
    }
}