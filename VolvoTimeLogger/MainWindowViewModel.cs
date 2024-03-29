﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VolvoTimeLogger
{
    internal class MainWindowViewModel : BindableBase
    {
        private IVolvoTimeService mService;
        private ISettingsWindow mSettingsWindow;
        private readonly ISettingsService mSettingsService;
        private ISelectionService mSelectionService;
        private string mJiraUrl;
        private TimeEntry mSelectedTimeEntry;
        private ImageSource mApplicationIcon;

        //private readonly string JiraBaseUrl = "https://jira.srv.volvo.com/browse/";

        public MainWindowViewModel(IVolvoTimeService service,
                                   ISelectionService selectionService,
                                   ISettingsWindow settingsWindow,
                                   ISettingsService settingsService)
        {
            mService = service;
            mSettingsWindow = settingsWindow;
            mSettingsService = settingsService;
            mSelectionService = selectionService;
            mSettingsService.SettingsEntryUpdated.Subscribe(HandleSettingsUpdated);

            AddNewEntryCommand = new RelayCommand(p => CanAddNewEntry, p => HandleAddNewEntry());
            DeleteTimeEntryCommand = new RelayCommand(p => true, p => HandleDeleteEntry(p));
            ExitCommand = new RelayCommand(p => true, p => HandleExitClicked());
            AboutCommand = new RelayCommand(p => true, p => HandleAboutClicked());
            SettingsCommand = new RelayCommand(p => true, p => HandleSettingsClicked());

            service.NewTimeEntryAdded.Subscribe(HandleNewTimeEntryAdded);
            service.TimeEntryUpdated.Subscribe(HandleTimeEntryUpdated);
            service.TimeEntryRemoved.Subscribe(HandleTimeEntryDeleted);

            TimeEntries = new ObservableCollection<TimeEntry>();
            foreach(var entry in service.QueryAllEntries())
            {
                TimeEntries.Add(entry);
            }
            CollectionView = new CollectionViewSource() { Source = TimeEntries };
            CollectionView.GroupDescriptions.Add(new PropertyGroupDescription("WeekNumber"));
            JiraUrl = "https://www.microsoft.com";
            if (settingsService.ApplicationIcon != null)
            {
                Uri iconUri = new Uri(settingsService.ApplicationIcon, UriKind.RelativeOrAbsolute);
                this.ApplicationIcon = BitmapFrame.Create(iconUri);
            }
        }

        private void HandleSettingsUpdated(SettingsEntry entry)
        {
            if (entry.ApplicationIcon != null)
            {
                Uri iconUri = new Uri(entry.ApplicationIcon, UriKind.RelativeOrAbsolute);
                this.ApplicationIcon = BitmapFrame.Create(iconUri);
            }
        }

        private void HandleTimeEntryDeleted(Guid id)
        {
            var itemToRemove = TimeEntries.Where(e => e.Id == id).FirstOrDefault();
            TimeEntries.Remove(itemToRemove);
        }

        private void HandleDeleteEntry(object param)
        {
            var entry = param as TimeEntry;
            mService.DeleteEntry(entry.Id);
        }

        private void HandleExitClicked()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void HandleAboutClicked()
        {
            var result = MessageBox.Show("Copyright 2023 (c) ZalcinSoft",
                                $"About - {Assembly.GetEntryAssembly().GetName().Name} - {Assembly.GetEntryAssembly().GetName().Version}",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
        }

        private void HandleSettingsClicked()
        {
            //var dlg = new SettingsWindow(mService);
            var dlg = new SettingsWindow();
            dlg.ShowDialog();
            //var result = MessageBox.Show("Settings not implemented yet...",
            //                    $"Settings - {Assembly.GetEntryAssembly().GetName().Name} - {Assembly.GetEntryAssembly().GetName().Version}",
            //                    MessageBoxButton.OK,
            //                    MessageBoxImage.Warning);
        }

        private void HandleNewTimeEntryAdded(TimeEntry entry)
        {
            TimeEntries.Add(entry);
        }

        private void HandleTimeEntryUpdated(TimeEntry entry)
        {
            var e = TimeEntries.Where(t => t.Id == entry.Id).FirstOrDefault();
            e.Timestamp = entry.Timestamp;
            e.NoOfHours = entry.NoOfHours;
            e.TicketReference = entry.TicketReference;
        }

        private bool _CanAddNewEntry()
        {
            return true; ;
        }

        private void HandleAddNewEntry()
        {
            var dlg = new NewEntryDialog(mService, mSettingsService);
            dlg.ShowDialog();
        }

        public string JiraUrl
        {
            get
            {
                return mJiraUrl;
            }
            set
            {
                SetProperty(ref mJiraUrl, value);
            }
        }

        public ImageSource ApplicationIcon
        {
            get
            {
                return mApplicationIcon;
            }
            set
            {
                SetProperty(ref mApplicationIcon, value);
            }
        }

        public TimeEntry SelectedTimeEntry
        {
            get
            {
                return mSelectedTimeEntry;
            }
            set
            {
                SetProperty(ref mSelectedTimeEntry, value);
                if(mSelectedTimeEntry != null)
                {
                    Console.WriteLine($"Selected item: {mSelectedTimeEntry.TicketReference}");
                    JiraUrl = mSettingsService.UrlRoot + mSelectedTimeEntry.TicketReference;
                }
                mSelectionService.Select(mSelectedTimeEntry != null ? mSelectedTimeEntry.Id : Guid.Empty);
            }
        }

        public CollectionViewSource CollectionView
        {
            get;
            set;
        }

        public ObservableCollection<TimeEntry> TimeEntries
        {
            get;
            set;
        }

        public ICommand DeleteTimeEntryCommand
        {
            get; set;
        }

        public ICommand AddNewEntryCommand
        {
            get;set;
        }

        public ICommand ExitCommand
        {
            get; set;
        }

        public ICommand AboutCommand
        {
            get; set;
        }

        public ICommand SettingsCommand
        {
            get; set;
        }

        public bool CanAddNewEntry
        {
            get
            {
                return true;
            }
        }
    }
}