using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VolvoTimeLogger
{
    public class SettingsWindowViewModel : BindableBase, IDataErrorInfo
    {
        private Window mParent;
        private readonly ISettingsService mSettingsService;
        private string mApplicationIcon;
        private string mUrlRoot;
        private ImageSource mApplicationIconBitmap;

        public SettingsWindowViewModel(Window parent,
                                       ISettingsService settingsService)
        {
            mParent = parent;
            mSettingsService = settingsService;
            mSettingsService.SettingsEntryUpdated.Subscribe(HandleSettingsUpdated);
            SaveConfigurationCommand = new RelayCommand(p => this.CanSaveConfiguration, p => this.HandleSaveConfiguration());
            CancelConfigurationCommand = new RelayCommand(p => true, p => this.HandleCancelConfiguration());
            BrowseForApplicationIconCommand = new RelayCommand(p => true, p => HandleBrowseForApplicationIconCommand());
            UrlRoot = mSettingsService.UrlRoot;
            ApplicationIcon = mSettingsService.ApplicationIcon;
            if (settingsService.ApplicationIcon != null)
            {
                Uri iconUri = new Uri(settingsService.ApplicationIcon, UriKind.RelativeOrAbsolute);
                this.ApplicationIconBitmap = BitmapFrame.Create(iconUri);
            }
        }

        private void HandleSettingsUpdated(SettingsEntry entry)
        {
            if (entry.ApplicationIcon != null)
            {
                Uri iconUri = new Uri(entry.ApplicationIcon, UriKind.RelativeOrAbsolute);
                ApplicationIcon = entry.ApplicationIcon;
            }
        }

        public ImageSource ApplicationIconBitmap
        {
            get
            {
                return mApplicationIconBitmap;
            }
            set
            {
                SetProperty(ref mApplicationIconBitmap, value);
            }
        }

        public string UrlRoot
        {
            get
            {
                return mUrlRoot;
            }
            set
            {
                SetProperty(ref mUrlRoot, value);
            }
        }

        public string ApplicationIcon
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

        private void HandleBrowseForApplicationIconCommand()
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                ApplicationIcon = ofd.FileName;
            }
        }

        private void HandleCancelConfiguration()
        {
            mParent.Hide();
        }

        private void HandleSaveConfiguration()
        {
            mSettingsService.Save(UrlRoot, ApplicationIcon);
            mParent.Hide();
        }

        public ICommand BrowseForApplicationIconCommand
        {
            get;
            set;
        }

        public ICommand SaveConfigurationCommand
        {
            get;
            set;
        }

        public ICommand CancelConfigurationCommand
        {
            get;
            set;
        }
        public bool CanSaveConfiguration
        {
            get
            {
                return ApplicationIconIsOK() && UrlRootIsOK();
            }
        }
        #region IDataErrorInfo Members

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        private bool UrlRootIsOK()
        {
            return string.IsNullOrEmpty(UrlRoot) || UrlRoot.StartsWith("http://") || UrlRoot.StartsWith("https://");
        }

        private bool ApplicationIconIsOK()
        {
            return string.IsNullOrEmpty(ApplicationIcon) || File.Exists(ApplicationIcon);
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;

                switch (columnName)
                {
                    case "UrlRoot":
                        result = UrlRootIsOK() ? null : "Url root is not valid";
                        break;

                    case "ApplicationIcon":
                        result = ApplicationIconIsOK() ? null : "File does not exist";
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
