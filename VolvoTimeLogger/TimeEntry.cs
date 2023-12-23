using System;
using System.Globalization;

namespace VolvoTimeLogger
{
    public class TimeEntry : BindableBase
    {
        private float mNoOfHours;

        public TimeEntry()
        {
        }

        public Guid Id
        {
            get;set;
        }

        public DateTime Timestamp
        {
            get;set;
        }

        public int WeekNumber
        {
            get
            {
                return GetWeekNumber(Timestamp);
            }
        }

        public float NoOfHours
        {
            get
            {
                return mNoOfHours;
            }
            set
            {
                SetProperty(ref mNoOfHours, value);
            }
        }

        public string TicketReference
        {
            get; set;
        }

        private static int GetWeekNumber(DateTime date)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }

    }
}