using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace VolvoTimeLogger
{
    public class SettingsWindowViewModel : BindableBase, IDataErrorInfo
    {
        private Window mParent;
        private readonly ISettingsService mSettingsService;
        private string mApplicationIcon;
        private string mUrlRoot;

        public SettingsWindowViewModel(Window parent,
                                       ISettingsService settingsService)
        {
            mParent = parent;
            mSettingsService = settingsService;
            SaveConfigurationCommand = new RelayCommand(p => this.CanSaveConfiguration, p => this.HandleSaveConfiguration());
            CancelConfigurationCommand = new RelayCommand(p => true, p => this.HandleCancelConfiguration());
            UrlRoot = mSettingsService.UrlRoot;
            ApplicationIcon = mSettingsService.ApplicationIcon;
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

        private void HandleCancelConfiguration()
        {
            mParent.Hide();
        }

        private void HandleSaveConfiguration()
        {
            mSettingsService.Save(UrlRoot, ApplicationIcon);
            mParent.Hide();
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
