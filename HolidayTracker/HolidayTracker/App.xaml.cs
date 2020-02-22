using System;
using Xamarin.Forms;
using HolidayTracker.Views;
using System.IO;
using HolidayTracker.Services;
using Microsoft.EntityFrameworkCore;
using Ninject;
using HolidayTracker.ViewModels;
using System.Reflection;

namespace HolidayTracker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var settings = new Ninject.NinjectSettings() { LoadExtensions = false };

            var kernel = new StandardKernel(settings);
            kernel.Load(Assembly.GetExecutingAssembly());

            MainPage = kernel.Get<Page>("Main");
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
