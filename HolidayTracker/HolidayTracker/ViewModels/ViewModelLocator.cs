using HolidayTracker.ViewModels;
using Ninject;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace HolidayTracker.ViewModels
{
    public static class ViewModelLocator
    {
        private static readonly StandardKernel kernel;

        static ViewModelLocator()
        {
            var settings = new Ninject.NinjectSettings() { LoadExtensions = false };

            kernel = new StandardKernel(settings);
            kernel.Load(Assembly.GetExecutingAssembly());
        }

        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached(
                "AutoWireViewModel",
                typeof(bool),
                typeof(ViewModelLocator),
                default(bool),
                propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable)
            => (bool)bindable.GetValue(AutoWireViewModelProperty);

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
            => bindable.SetValue(AutoWireViewModelProperty, value);

        //    public static void RegisterSingleton<TInterface, T>() where TInterface : class where T : class, TInterface
        //=> Container.RegisterSingleton<TInterface, T>();

        public static T Resolve<T>() where T : class
            => kernel.Get<T>();

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as Element;

            var viewType = view?.GetType();
            if (viewType?.FullName == null)
            {
                return;
            }

            var viewName = viewType.FullName.Replace("View", "ViewModel");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewAssemblyName);

            var viewModelType = Type.GetType(viewModelName);
            if (viewModelType == null)
            {
                return;
            }

            var viewModel = kernel.Get(viewModelType);
            view.BindingContext = viewModel;
        }
    }
}
