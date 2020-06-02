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
            var publicHolidays = _context.PublicHolidays.Where(ph => ph.Date >= holiday.Start && ph.Date <= holiday.End).ToList();
            var firstHolidayPeriod = _context.HolidayPeriods.OrderByDescending(hp => hp.Start).FirstOrDefault(hp => hp.Start < holiday.Start);

            var newHolidayGUID = Guid.NewGuid();

            var WorkWeekends = bool.Parse(GetSetting("WorkWeekends"));
            var WorkPublicHolidays = bool.Parse(GetSetting("WorkPublicHolidays"));


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
                    WholeHolidayGuid = newHolidayGUID,
                    NumDays = Calculate.NumDaysInHoliday(holiday.Start, firstHolidayPeriod.End, publicHolidays, WorkWeekends, WorkPublicHolidays),
                    Split = true
                };
                var holpt2 = new Holiday()
                {
                    Description = holiday.Description,
                    Start = nextHolidayPeriod.Start,
                    End = holiday.End,
                    WholeHolidayGuid = newHolidayGUID,
                    NumDays = Calculate.NumDaysInHoliday(nextHolidayPeriod.Start, holiday.End, publicHolidays, WorkWeekends, WorkPublicHolidays),
                    Split = true
                };
                firstHolidayPeriod.Holidays.Add(holpt1);
                nextHolidayPeriod.Holidays.Add(holpt2);
            }
            else
            {
                holiday.NumDays = Calculate.NumDaysInHoliday(holiday.Start, holiday.End, publicHolidays, WorkWeekends, WorkPublicHolidays);
                holiday.WholeHolidayGuid = newHolidayGUID;
                firstHolidayPeriod.Holidays.Add(holiday);
            }
            OnDataUpdate(EventArgs.Empty);
        }

        public void UpdateHoliday(Holiday holiday)
        {
            var publicHolidays = _context.PublicHolidays.Where(ph => ph.Date >= holiday.Start && ph.Date <= holiday.End).ToList();

            var WorkWeekends = bool.Parse(GetSetting("WorkWeekends"));
            var WorkPublicHolidays = bool.Parse(GetSetting("WorkPublicHolidays"));

            var startHolidayPeriod = _context.HolidayPeriods.Include(hp => hp.Holidays).OrderByDescending(hp => hp.Start).FirstOrDefault(hp => hp.Start < holiday.Start);

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
                            holidayInPeriod.NumDays = Calculate.NumDaysInHoliday(holiday.Start, holiday.End, publicHolidays, WorkWeekends, WorkPublicHolidays);
                        }
                        else
                        {
                            var holpt2 = new Holiday()
                            {
                                Description = holiday.Description,
                                Start = holidayPeriod.Start,
                                End = holiday.End,
                                WholeHolidayGuid = holiday.WholeHolidayGuid,
                                NumDays = Calculate.NumDaysInHoliday(holidayPeriod.Start, holiday.End, publicHolidays, WorkWeekends, WorkPublicHolidays),
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
                            NumDays = Calculate.NumDaysInHoliday(holiday.Start, holiday.End, publicHolidays, WorkWeekends, WorkPublicHolidays),
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
                thisHoliday.NumDays = Calculate.NumDaysInHoliday(holiday.Start, holiday.End, publicHolidays, WorkWeekends, WorkPublicHolidays);
                thisHoliday.Split = false;
            }

            OnDataUpdate(EventArgs.Empty);
        }

        public HolidayPeriod GetHolidayPeriod(DateTime date)
        {
            if (Global.CurrentHolidayPeriod == null)
            {
                Global.CurrentHolidayPeriod = _context.HolidayPeriods.Include(hp => hp.Holidays).FirstOrDefault(hp => hp.Start <= date && hp.End >= date);
            }
            return Global.CurrentHolidayPeriod;
        }

        public bool CheckForHolidayPeriodOverlap(HolidayPeriod possibleHolidayPeriod)
        {
            foreach (var holidayPeriod in _context.HolidayPeriods)
            {
                if ((holidayPeriod.Start < possibleHolidayPeriod.Start && holidayPeriod.End > possibleHolidayPeriod.Start) || (holidayPeriod.Start < possibleHolidayPeriod.End && holidayPeriod.End > possibleHolidayPeriod.End))
                    return true;
            }
            return false;
        }

        public HolidayPeriod GetHolidayPeriod(int id)
        {
            return _context.HolidayPeriods.FirstOrDefault(hp => hp.ID == id);
        }

        private void CreateHolidayPeriod(DateTime PeriodStart, DateTime PeriodEnd, int PeriodNumHolidays = 0)
        {
            if (PeriodNumHolidays == 0)
            {
                PeriodNumHolidays = Convert.ToInt32(GetSetting("NumHolidays"));
            }
            _context.HolidayPeriods.Add(new HolidayPeriod()
            {
                Start = PeriodStart,
                End = PeriodEnd,
                NumHolidays = PeriodNumHolidays
            });
            OnDataUpdate(EventArgs.Empty);
        }

        public void CreateHolidayPeriod(HolidayPeriod newHolidayPeriod)
        {
            _context.HolidayPeriods.Add(newHolidayPeriod);
            OnDataUpdate(EventArgs.Empty);
        }

        public void CreateHolidayPeriod()
        {
            var StartMonth = Convert.ToInt32(GetSetting("HolidayPeriodStart"));
            int EndMonth;
            DateTime NewPeriodStart;
            DateTime NewPeriodEnd;

            HolidayPeriod previousHolidayPeriod;
            if (_context.HolidayPeriods.Any())
            {
                previousHolidayPeriod = _context.HolidayPeriods.OrderBy(hp => hp.End).Last();
                NewPeriodStart = previousHolidayPeriod.End.AddDays(1);
                NewPeriodEnd = previousHolidayPeriod.End.AddDays(Convert.ToInt32(GetSetting("PeriodLength")));
            }
            else
            {
                var EndYear = DateTime.Now.Year;
                int lastDayOfEndMonth = 31;
                if (StartMonth > 1)
                {
                    EndMonth = StartMonth - 1;
                    EndYear = DateTime.Now.Year + 1;
                    lastDayOfEndMonth = DateTime.DaysInMonth(DateTime.Now.Year, EndMonth);
                }
                else
                {
                    EndMonth = 12;
                }
                NewPeriodStart = new DateTime(DateTime.Now.Year, EndMonth, 1);
                NewPeriodEnd = new DateTime(EndYear, EndMonth, lastDayOfEndMonth);
            }
            _context.HolidayPeriods.Add(new HolidayPeriod()
            {
                Start = NewPeriodStart,
                End = NewPeriodEnd,
                NumHolidays = 25
            });
            OnDataUpdate(EventArgs.Empty);
        }

        public string GetSetting(string Key)
        {
            return _context.Settings.FirstOrDefault(s => s.Key == Key).Value;
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
                    Value = Value,
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

        public void AddPublicHoliday(PublicHoliday publicHoliday)
        {
            _context.PublicHolidays.Add(publicHoliday);
            _context.Save();
        }

        public IEnumerable<PublicHoliday> GetPublicHolidays(DateTime start, DateTime end)
        {
            return _context.PublicHolidays.Where(ph => ph.Date >= start && ph.Date <= end);
        }

        public void CreateTestData()
        {
            Setup();
            _context.HolidayPeriods.RemoveRange(_context.HolidayPeriods);
            CreateHolidayPeriod();
            var hol = new Holiday()
            {
                Description = "Test",
                Start = DateTime.Now.Date,
                End = DateTime.Now.AddDays(14),
                Split = false
            };
            CreateHoliday(hol);
        }

        public void Setup()
        {
            if (!_context.Settings.Any())
            {
                UpsertSetting("PeriodLength", "365");
                UpsertSetting("WorkWeekends", "false");
                UpsertSetting("WorkPublicHolidays", "false");
                UpsertSetting("HolidayPeriodStart", "4");
                UpsertSetting("LastPublicHolidayUpdate", DateTime.Now.ToString());
            }
        }

        public List<HolidayPeriod> GetHolidayPeriods()
        {
            return _context.HolidayPeriods.ToList();
        }
    }
}
