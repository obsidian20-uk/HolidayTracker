using HolidayTracker.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace HolidayTracker.Services
{
    public class WebData
    {
        public IDataAccessService DataAccessService { get; set; }

        public WebData(IDataAccessService _DataAccessService)
        {
            DataAccessService = _DataAccessService;

        }

        // Using System.Net
        // NewtonSoft JSON.NET
        private BankHolidays GetUkGovBankHolidays()
        {
            string apiEndPoint = "https://www.gov.uk/bank-holidays.json";
            BankHolidays apiHolidays = null;

            using (var wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string response = wb.DownloadString(apiEndPoint);
                apiHolidays = JsonConvert.DeserializeObject<BankHolidays>(response);
            }
            return apiHolidays;
        }
        public void UpdatePublicHolidays(bool Forced = false)
        {
            var LastPublicHolidayUpdate = DateTime.Parse(DataAccessService.GetSetting("LastPublicHolidayUpdate"));
            if (Forced || DateTime.Now.Subtract(LastPublicHolidayUpdate).TotalDays > 7)
            {
                var country = DataAccessService.GetSetting("Country");
                var existingHolidays = DataAccessService.GetPublicHolidays(DateTime.Now, DateTime.MaxValue).ToList();
                List<Event> events = new List<Event>();
                switch (country)
                {
                    case "Eng":
                        events = GetUkGovBankHolidays().EnglandAndWales.Events;
                        break;
                    case "Wal":
                        events = GetUkGovBankHolidays().EnglandAndWales.Events;
                        break;
                    case "Scot":
                        events = GetUkGovBankHolidays().Scotland.Events;
                        break;
                    case "NRL":
                        events = GetUkGovBankHolidays().NorthernIreland.Events;
                        break;
                    default:
                        break;
                }
                DataAccessService.TidyPublicHolidays();
                foreach (var BH in events)
                {
                    if (BH.Date >= DateTime.Now && !existingHolidays.Any(eh => eh.Date == BH.Date))
                    {
                        var publicHoliday = new PublicHoliday()
                        {
                            Date = BH.Date,
                            Description = BH.Title
                        };
                        DataAccessService.AddPublicHoliday(publicHoliday);
                    }
                }
                DataAccessService.UpsertSetting("LastPublicHolidayUpdate", DateTime.Now.ToString());
            }
        }
    }
}
