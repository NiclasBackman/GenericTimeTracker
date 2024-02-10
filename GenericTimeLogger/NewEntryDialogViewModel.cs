using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace VolvoTimeLogger
{
    internal class NewEntryDialogViewModel : BindableBase, IDataErrorInfo
    {
        private IVolvoTimeService service;
        private readonly Window parent;
        private DateTime mTimestamp;
        private float mNumberOfHours;
        private string mJiraRef;

        public NewEntryDialogViewModel(IVolvoTimeService service, Window parent)
        {
            this.service = service;
            this.parent = parent;
            SaveNewEntryCommand = new RelayCommand(p => this.CanSaveNewEntry, p => this.HandleSaveNewEntry());
            CancelNewEntryCommand = new RelayCommand(p => true, p => this.HandleCancelNewEntry());
            Timestamp = DateTime.Now;
            NumberOfHours = 0;
            JiraRef = string.Empty;
        }

        private void HandleSaveNewEntry()
        {
            service.AddNewEntry(mTimestamp, mNumberOfHours, mJiraRef);
            parent.Close();
        }

        private void HandleCancelNewEntry()
        {
            parent.Close();
        }

        public bool CanSaveNewEntry
        {
            get
            {
                return NumberOfHoursIsValid() && JiraRefIsValid();
            }
        }
        public ICommand SaveNewEntryCommand
        {
            get;
            set;
        }

        public ICommand CancelNewEntryCommand
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

        private bool NumberOfHoursIsValid()
        {
            return this.NumberOfHours > 0.0;
        }

        private bool JiraRefIsValid()
        {
            return string.IsNullOrEmpty(this.JiraRef) == false;
        }

    }
}