using System;
using System.Collections.Generic;

namespace VolvoTimeLogger
{
    public interface IVolvoTimeService
    {
        ObservableProperty<TimeEntry> NewTimeEntryAdded { get; }

        ObservableProperty<TimeEntry> TimeEntryUpdated { get; }

        ObservableProperty<Guid> TimeEntryRemoved { get; }

        List<TimeEntry> QueryAllEntries();

        void AddNewEntry(DateTime timestamp, float noOfHours, string jiraRef);

        TimeEntry QueryEntry(Guid id);

        void UpdateEntry(Guid id, DateTime timestamp, float noOfHours, string ticketReference);

        void DeleteEntry(Guid id);
    }
}