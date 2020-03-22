using HolidayTracker.Models;
using HolidayTracker.Services;
using HolidayTracker.Views;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HolidayTracker.Modules
{
    public class HolidayTrackerCoreModules : NinjectModule
    {
        public override void Load()
        {
            Bind<IDatabaseContext>().To<HolidayDatabaseContext>();
            Bind<IDataAccessService>().To<EFDataAccessService>();
            Bind<MasterDetailPage>().To<MainView>().Named("Main");
            Bind<ContentPage>().To<HolidaysView>().Named("HolidayList");
        }
    }
}