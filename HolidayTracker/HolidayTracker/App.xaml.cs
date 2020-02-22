using System;
using Xamarin.Forms;
using HolidayTracker.Views;
using System.IO;
using HolidayTracker.Services;

namespace HolidayTracker
{
    public partial class App : Application
    {
        public HolidayDatabaseContext holidayDatabaseContext = new HolidayDatabaseContext();

        public App()
        {
            InitializeComponent();

            holidayDatabaseContext.Database.EnsureCreated();

            MainPage = new MainPage(holidayDatabaseContext);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
