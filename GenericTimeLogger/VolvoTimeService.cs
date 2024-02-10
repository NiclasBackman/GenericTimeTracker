using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolvoTimeLogger
{
    public static class datetimeextensions
    {
        public static DateTime startofweek(this DateTime dt, DayOfWeek startofweek)
        {
            int diff = dt.DayOfWeek - startofweek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }
    }

    public class VolvoTimeService : IVolvoTimeService
    {
        private List<TimeEntry> mAllEntries;
        private JsonSerializer mSerializer;
        private readonly string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "timelog.json");


        public VolvoTimeService()
        {
            NewTimeEntryAdded = new ObservableProperty<TimeEntry>();
            TimeEntryUpdated = new ObservableProperty<TimeEntry>();
            TimeEntryRemoved = new ObservableProperty<Guid>();
            mAllEntries = new List<TimeEntry>();
            mSerializer = new JsonSerializer();
            if(File.Exists(FilePath))
            {
                using (StreamReader file = File.OpenText(FilePath))
                {
                    mAllEntries = (List<TimeEntry>)mSerializer.Deserialize(file, typeof(List<TimeEntry>));
                }
            }
        }

        public ObservableProperty<TimeEntry> NewTimeEntryAdded
        {
            get;
        }

        public ObservableProperty<TimeEntry> TimeEntryUpdated
        {
            get;
        }

        public ObservableProperty<Guid> TimeEntryRemoved
        {
            get;
        }

        public List<TimeEntry> QueryAllEntries()
        {
            return mAllEntries.ToList();
        }

        public void AddNewEntry(DateTime timestamp, float noOfHours, string jiraRef)
        {
            var entry = new TimeEntry
            {
                Id = Guid.NewGuid(),
                Timestamp = timestamp,
                NoOfHours = noOfHours,
                TicketReference = jiraRef
            };

            mAllEntries.Add(entry);
            NewTimeEntryAdded.Publish(entry);
            //var grouped = mAllEntries.GroupBy(i => i.Timestamp.startofweek(DayOfWeek.Monday));
            //foreach (var group in grouped)
            //{
            //    Console.WriteLine("Week Number: " + GetWeekNumber(group.Key));
            //    foreach (var e in group)
            //        Console.WriteLine("* " + e.Timestamp);
            //}
            Save();
        }

        private void Save()
        {
            using (StreamWriter sw = new StreamWriter(FilePath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                mSerializer.Serialize(writer, mAllEntries);
            }
        }

        private int GetWeekNumber(DateTime date)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }

        public TimeEntry QueryEntry(Guid id)
        {
            return mAllEntries.Where(e => e.Id == id).FirstOrDefault();
        }

        public void UpdateEntry(Guid id, DateTime timestamp, float noOfHours, string ticketReference)
        {
            var entry = mAllEntries.Where(e => e.Id == id).FirstOrDefault();
            if(entry == null)
            {
                throw new ArgumentException($"Unable to locate entry with id {id}");
            }
            entry.Timestamp = timestamp;
            entry.NoOfHours = noOfHours;
            entry.TicketReference = ticketReference;
            TimeEntryUpdated.Publish(entry);
            Save();
        }

        public void DeleteEntry(Guid id)
        {
            mAllEntries.RemoveAll(e => e.Id == id);
            TimeEntryRemoved.Publish(id);
            Save();
        }
    }
}
