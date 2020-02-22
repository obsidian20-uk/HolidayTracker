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
            Bind(typeof(IDataAccess<>)).To(typeof(HolidayDatabaseAccess)).Named("Holiday");
            Bind(typeof(IDataAccess<>)).To(typeof(HolidayAllowanceDatabaseAccess)).Named("HolidayAllowance");
            Bind<Page>().To<MainView>().Named("Main");
        }
    }
}
