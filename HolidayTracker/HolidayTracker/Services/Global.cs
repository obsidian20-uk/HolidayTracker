using Ninject;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HolidayTracker.Services
{
    public class Global
    {
        public static IKernel kernel;

        public static IKernel CreateKernel()
        {
            var settings = new Ninject.NinjectSettings() { LoadExtensions = false };
            kernel = new StandardKernel(settings);
            kernel.Load(Assembly.GetExecutingAssembly());
            return kernel;
        }
    }
}
