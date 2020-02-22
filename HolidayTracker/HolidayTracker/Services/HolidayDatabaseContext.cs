using HolidayTracker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using Xamarin.Forms;

namespace HolidayTracker.Services
{
    public class HolidayDatabaseContext : DbContext, IDatabaseContext
    {

        private const string databaseName = "HolidayTracker.db";

        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<HolidayAllowance> HolidayAllowances { get; set; }

        public HolidayDatabaseContext()
        {
        }

        public void Initialise()
        {
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            String databasePath = "";
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    SQLitePCL.Batteries_V2.Init();
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", databaseName); ;
                    break;
                case Device.Android:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), databaseName);
                    break;
                default:
                    throw new NotImplementedException("Platform not supported");
            }
            // Specify that we will use sqlite and the path of the database here
            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }
    }
}
