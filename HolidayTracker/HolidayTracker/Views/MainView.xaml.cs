﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HolidayTracker.Models;

namespace HolidayTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : MasterDetailPage
    {
        public MainView()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        public MainView(Page page)
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
            Detail = new NavigationPage(page);
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainMenuItem;
            if (item == null)
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType);
            //page.Title = item.Title;

            Detail = new NavigationPage(page)
            {
                BarBackgroundColor = Color.FromHex("#ECFFFB"),
                BarTextColor = Color.Black
        };
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}