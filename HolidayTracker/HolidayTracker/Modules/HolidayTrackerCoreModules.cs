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
            Bind<IDataAccess<Holiday>>().To<HolidayDatabaseAccess>();
            Bind<IDataAccess<HolidayAllowance>>().To<HolidayAllowanceDatabaseAccess>();
            Bind<Page>().To<MainView>().Named("Main");
        }
    }
}
