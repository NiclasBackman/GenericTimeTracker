using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace VolvoTimeLogger
{
    public class TimeEntryUserControlViewModel : BindableBase, IDataErrorInfo
    {
        private IVolvoTimeService volvoService;
        private DateTime mTimestamp;
        private float mNumberOfHours;
        private string mJiraRef;
        private TimeEntry mCurrentEntry;

        public TimeEntryUserControlViewModel(IVolvoTimeService volvoService, ISelectionService selectionService)
        {
            this.volvoService = volvoService;
            selectionService.SelectionChanged.Subscribe(HandleSelectionChanged);
            volvoService.TimeEntryUpdated.Subscribe(HandleTimeEntryUpdated);
            volvoService.TimeEntryRemoved.Subscribe(HandleTimeEntryRemoved);
            CurrentEntry = null;
            SaveEntryCommand = new RelayCommand(p => CanSaveEntry, p => this.HandleSaveEntry());
            CancelEntryCommand = new RelayCommand(p => CanCancelEntry, p => this.HandleCancelEntry());
        }

        private void HandleTimeEntryRemoved(Guid id)
        {
            if(id == CurrentEntry.Id)
            {
                CurrentEntry = null;
            }
        }

        private void HandleTimeEntryUpdated(TimeEntry entry)
        {
            if(CurrentEntry.Id == entry.Id)
            {
                //SaveEntryCommand.RaiseCanExecuteChanged();
            }
        }

        private void HandleSaveEntry()
        {
            volvoService.UpdateEntry(CurrentEntry.Id, Timestamp, NumberOfHours, JiraRef);
        }

        private void HandleCancelEntry()
        {
            Timestamp = mCurrentEntry.Timestamp;
            NumberOfHours = mCurrentEntry.NoOfHours;
            JiraRef = mCurrentEntry.TicketReference;
        }

        public bool CanSaveEntry
        {
            get
            {
                return isChanged() && NumberOfHoursIsValid() == true && JiraRefIsValid() == true;
            }
        }

        public bool CanCancelEntry
        {
            get
            {
                var res = isChanged() || NumberOfHoursIsValid() == false || JiraRefIsValid() == false;
                // Console.WriteLine($"CanCancelEntry: {res}, isChanged(): {isChanged()}, NumberOfHoursIsValid(): {NumberOfHoursIsValid()}, JiraRefIsValid(): {JiraRefIsValid()}");
                return isChanged() || NumberOfHoursIsValid() == false || JiraRefIsValid() == false;
            }
        }

        private void HandleSelectionChanged(Guid id)
        {
            Console.WriteLine($"Selection changed: {id}");
            if(isChanged() && mCurrentEntry != null)
            {
                var result = MessageBox.Show("You are navigating away from form and there is unsaved data, do you want to save it before you leave?",
                    $"Time Entry - {this.mCurrentEntry.Timestamp.ToShortDateString()}",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    volvoService.UpdateEntry(CurrentEntry.Id, Timestamp, NumberOfHours, JiraRef);
                }
            }

            CurrentEntry = volvoService.QueryEntry(id);
            if(CurrentEntry != null)
            {
                Timestamp = mCurrentEntry.Timestamp;
                NumberOfHours = mCurrentEntry.NoOfHours;
                JiraRef = mCurrentEntry.TicketReference;
            }
        }

        public ICommand SaveEntryCommand
        {
            get;
            set;
        }

        public ICommand CancelEntryCommand
        {
            get;
            set;
        }

        public DateTime Timestamp
        {
            get
            {
                return mTimestamp;
            }
            set
            {
                SetProperty(ref mTimestamp, value);
            }
        }

        public float NumberOfHours
        {
            get
            {
                return mNumberOfHours;
            }
            set
            {
                SetProperty(ref mNumberOfHours, value);
            }
        }

        public string JiraRef
        {
            get
            {
                return mJiraRef;
            }
            set
            {
                SetProperty(ref mJiraRef, value);
            }
        }

        public TimeEntry CurrentEntry
        {
            get
            {
                return mCurrentEntry;
            }
            set
            {
                SetProperty(ref mCurrentEntry, value);
            }
        }

        private bool isChanged()
        {
            return mCurrentEntry == null || (mCurrentEntry.Timestamp != Timestamp ||
                   mCurrentEntry.NoOfHours != NumberOfHours ||
                   mCurrentEntry.TicketReference != JiraRef);
        }

        private bool NumberOfHoursIsValid()
        {
            //Console.WriteLine($"this.NumberOfHours: {this.NumberOfHours}");
            return this.NumberOfHours > 0.0;
        }

        private bool JiraRefIsValid()
        {
            return string.IsNullOrEmpty(this.JiraRef) == false;
        }

        #region IDataErrorInfo Members

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;

                switch (columnName)
                {
                    case "Timestamp":
                        result = null;
                        break;

                    case "NumberOfHours":
                        result = this.NumberOfHours <= 0.0 ? "Number of hours must be greater than 0" : null;
                        break;

                    case "JiraRef":
                        result = string.IsNullOrEmpty(this.JiraRef) ? "You must enter a value" : null;
                        break;

                    default:
                        break;
                }

                return result;
            }
        }

        #endregion
    }
}
