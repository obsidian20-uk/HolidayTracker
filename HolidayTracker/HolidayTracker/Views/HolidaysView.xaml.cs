using HolidayTracker.Models;
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
    public partial class HolidaysView : ContentPage
    {
        private HolidayPeriod holidayPeriod;

        public HolidaysView()
        {
            InitializeComponent();
        }

        public HolidaysView(HolidayPeriod holidayPeriod)
        {
            this.holidayPeriod = holidayPeriod;
        }
    }
}