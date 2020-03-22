using HolidayTracker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace HolidayTracker.Services
{
    public class HolidayDatabaseContext : DbContext, IDatabaseContext
    {

        private const string databaseName = "HolidayTracker.db";

        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<HolidayPeriod> HolidayPeriods { get; set; }
        public DbSet<Setting> Settings { get; set; }

        public HolidayDatabaseContext()
        {
            Initialise();
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
                case Device.Android:
                    var sqlitePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"ObsidianSoftware\HolidayTracker");
                    Directory.CreateDirectory(sqlitePath);
                    var fileName = $"{sqlitePath}\\{databaseName}";
                    if (!File.Exists(fileName))
                    {
                        File.Create(fileName);
                    }
                    optionsBuilder.UseSqlite($"Data Source={fileName}");
                    break;
                default:
                    throw new NotImplementedException("Platform not supported");
                    break;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HolidayPeriod>()
            .HasMany(hp => hp.Holidays)
            .WithOne();
        }

        public void Save()
        {
            this.SaveChanges();
        }
    }
}
