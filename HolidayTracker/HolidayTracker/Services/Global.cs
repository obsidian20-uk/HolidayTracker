using HolidayTracker.Models;
using Ninject;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Text;

namespace HolidayTracker.Services
{
    public class Global
    {
        public static HolidayPeriod CurrentHolidayPeriod { get; set; }

        public static IKernel kernel;

        public static IKernel CreateKernel()
        {
            var settings = new Ninject.NinjectSettings() { LoadExtensions = false };
            kernel = new StandardKernel(settings);
            kernel.Load(Assembly.GetExecutingAssembly());
            return kernel;
        }

        public static Dictionary<string, string> Countries = new Dictionary<string, string>()
        {
            {"Eng", "England"},
            {"Wal", "Wales"},
            {"NRL", "Northern Ireland"},
            {"Scot", "Scotland"}
        };
    }
}
