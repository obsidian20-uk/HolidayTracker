﻿using HolidayTracker.Models;
using HolidayTracker.ViewModels;
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
    public partial class EditHolidaysView : ContentPage
    {
        public EditHolidaysView()
        {
            //this.BindingContext = new EditHolidaysViewModel(ref holiday);

            InitializeComponent();
        }
    }
}