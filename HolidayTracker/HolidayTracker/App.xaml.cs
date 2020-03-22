using System;
using Xamarin.Forms;
using HolidayTracker.Views;
using System.IO;
using HolidayTracker.Services;
using Microsoft.EntityFrameworkCore;
using Ninject;
using HolidayTracker.ViewModels;
using System.Reflection;
using HolidayTracker.Models;
using System.Threading.Tasks;

namespace HolidayTracker
{
    public partial class App : Application
    {
        public App()
        {
            Global.kernel = Global.CreateKernel();

            MainPage = Global.kernel.Get<MasterDetailPage>("Main");

            InitializeComponent();
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
