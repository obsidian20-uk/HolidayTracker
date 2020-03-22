using HolidayTracker.Models;
using HolidayTracker.Modules;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace HolidayTracker.Services
{
    public class EFDataAccessService : IDataAccessService
    {
        private readonly IDatabaseContext _context;

        public EFDataAccessService(IDatabaseContext context)
        {
            _context = context;

        }

        public event EventHandler DataUpdate;

        public IEnumerable<Holiday> GetAll()
        {
            var AllHolidays = new List<Holiday>();
            foreach (var holidayperiod in _context.HolidayPeriods)
            {
                AllHolidays.AddRange(holidayperiod.Holidays);
            }
            return AllHolidays;
        }

        public IEnumerable<Holiday> GetHolidaysInPeriod(int holidayPeriodID)
        {
            return _context.HolidayPeriods.Include(hp => hp.Holidays).FirstOrDefault(hp => hp.ID == holidayPeriodID).Holidays;
        }

        public void CreateHoliday(Holiday holiday)
        {
            var firstHolidayPeriod = _context.HolidayPeriods.OrderByDescending(hp => hp.Start).FirstOrDefault(hp => hp.Start < holiday.Start);
            if (firstHolidayPeriod.Holidays == null)
            {
                firstHolidayPeriod.Holidays = new List<Holiday>();
            }
            if (holiday.End > firstHolidayPeriod.End)
            {
                var nextHolidayPeriod = _context.HolidayPeriods.OrderByDescending(hp => hp.Start).FirstOrDefault(hp => hp.End >= holiday.Start);
                nextHolidayPeriod.Holidays = new List<Holiday>();
                var holpt1 = new Holiday()
                {
                    Description = holiday.Description,
                    Start = holiday.Start,
                    End = firstHolidayPeriod.End,
                    WholeHolidayGuid = holiday.WholeHolidayGuid,
                    Split = true
                };
                var holpt2 = new Holiday()
                {
                    Description = holiday.Description,
                    Start = nextHolidayPeriod.Start,
                    End = holiday.End,
                    WholeHolidayGuid = holiday.WholeHolidayGuid,
                    Split = true
                };
                firstHolidayPeriod.Holidays.Add(holpt1);
                nextHolidayPeriod.Holidays.Add(holpt2);
            }
            else
            {
                firstHolidayPeriod.Holidays.Add(holiday);
            }
            OnDataUpdate(EventArgs.Empty);
        }

        public void UpdateHoliday(Holiday holiday)
        {
            var startHolidayPeriod = _context.HolidayPeriods.OrderByDescending(hp => hp.Start).FirstOrDefault(hp => hp.Start < holiday.Start);

            if (!holiday.Split && holiday.End > startHolidayPeriod.End)
            {
                holiday.Split = true;
            }

            if (holiday.Split)
            {
                foreach (var holidayPeriod in _context.HolidayPeriods.Where(hp => hp.Start > startHolidayPeriod.End))
                {
                    if (holiday.End < holidayPeriod.Start)
                    {
                        holidayPeriod.Holidays.RemoveAll(h => h.WholeHolidayGuid == holiday.WholeHolidayGuid);
                    }
                    else if (holiday.End <= holidayPeriod.End)
                    {
                        if (holidayPeriod.Holidays.Any(h => h.WholeHolidayGuid == holiday.WholeHolidayGuid))
                        {
                            var holidayInPeriod = holidayPeriod.Holidays.FirstOrDefault(h => h.WholeHolidayGuid == holiday.WholeHolidayGuid);
                            holidayInPeriod.End = holiday.End;
                        }
                        else
                        {
                            var holpt2 = new Holiday()
                            {
                                Description = holiday.Description,
                                Start = holidayPeriod.Start,
                                End = holiday.End,
                                WholeHolidayGuid = holiday.WholeHolidayGuid,
                                Split = true
                            };
                            holidayPeriod.Holidays.Add(holpt2);
                        }
                    }
                    else
                    {
                        var holpt2 = new Holiday()
                        {
                            Description = holiday.Description,
                            Start = holiday.Start,
                            End = holidayPeriod.End,
                            WholeHolidayGuid = holiday.WholeHolidayGuid,
                            Split = true
                        };
                        holidayPeriod.Holidays.Add(holpt2);
                    }
                }
            }
            else
            {
                var thisHoliday = startHolidayPeriod.Holidays.FirstOrDefault(h => h.WholeHolidayGuid == holiday.WholeHolidayGuid);
                thisHoliday.Description = holiday.Description;
                thisHoliday.Start = holiday.Start;
                thisHoliday.End = holiday.End;
                thisHoliday.Split = false;
            }

            OnDataUpdate(EventArgs.Empty);
        }

        public HolidayPeriod GetHolidayPeriod(DateTime date)
        {
            if (!_context.HolidayPeriods.Any(hp => hp.Start <= date && hp.End > date))
            {
                CreateHolidayPeriod();
            }
            return _context.HolidayPeriods.Include(hp => hp.Holidays).FirstOrDefault(hp => hp.Start <= date && hp.End >= date);
        }

        public HolidayPeriod GetHolidayPeriod(int id)
        {
            if (!_context.HolidayPeriods.Any(hp => hp.ID == id))
            {
                CreateHolidayPeriod();
            }
            return _context.HolidayPeriods.FirstOrDefault(hp => hp.ID == id);
        }

        private void CreateHolidayPeriod(DateTime PeriodStart, DateTime PeriodEnd, int PeriodNumHolidays = 0)
        {
            if (PeriodNumHolidays == 0)
            {
                PeriodNumHolidays = Convert.ToInt32(GetSetting("NumHolidays").Value);
            }
            _context.HolidayPeriods.Add(new HolidayPeriod()
            {
                Start = PeriodStart,
                End = PeriodEnd,
                NumHolidays = PeriodNumHolidays
            });
            OnDataUpdate(EventArgs.Empty);
        }

        public void CreateHolidayPeriod()
        {
            DateTime NewPeriodStart;
            DateTime NewPeriodEnd;

            var previousHolidayPeriod = _context.HolidayPeriods.OrderBy(hp => hp.End).LastOrDefault();
            if (previousHolidayPeriod == null)
            {
                NewPeriodStart = new DateTime(DateTime.Now.Year, 1, 1);
                NewPeriodEnd = new DateTime(DateTime.Now.Year, 12, 31);
            }
            else
            {
                NewPeriodStart = previousHolidayPeriod.End.AddDays(1);
                NewPeriodEnd = previousHolidayPeriod.End.AddDays(Convert.ToInt32(GetSetting("PeriodLength").Value));

            }
            _context.HolidayPeriods.Add(new HolidayPeriod()
            {
                Start = NewPeriodStart,
                End = NewPeriodEnd,
                NumHolidays = 25
            });
            OnDataUpdate(EventArgs.Empty);
        }

        public Setting GetSetting(string Key)
        {
            return _context.Settings.FirstOrDefault(s => s.Key == Key);
        }

        public void UpsertSetting(string Key, string Value)
        {
            if (_context.Settings.Any(s => s.Key == Key))
            {
                _context.Settings.FirstOrDefault(s => s.Key == Key).Value = Value;
            }
            else
            {
                _context.Settings.Add(new Setting()
                {
                    Key = Key,
                    Value = Value
                });
            }
            OnDataUpdate(EventArgs.Empty);
        }

        public void DeleteHoliday(Holiday holiday)
        {
            var FirstHolidaysPeriod = _context.HolidayPeriods.FirstOrDefault(hp => hp.Holidays.Any(h => h.WholeHolidayGuid == holiday.WholeHolidayGuid));
            foreach (var holidayPeriod in _context.HolidayPeriods)
            {
                holidayPeriod.Holidays.RemoveAll(h => h.WholeHolidayGuid == holiday.WholeHolidayGuid);
            }
            OnDataUpdate(EventArgs.Empty);
        }

        protected virtual void OnDataUpdate(EventArgs e)
        {
            _context.Save();
            EventHandler handler = DataUpdate;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void CreateTestData()
        {
            var hol = new Holiday()
            {
                Description = "Test",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(5),
                Split = false,
                WholeHolidayGuid = new Guid()
            };
            CreateHoliday(hol);
        }


    }
}
