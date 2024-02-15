using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolvoTimeLogger
{
    public class SettingsService : ISettingsService
    {
        private IApplicationConstants mAppConstants;
        private JsonSerializer mSerializer;
        private SettingsEntry mSettingsEntry;

        public SettingsService(IApplicationConstants appConstants)
        {
            this.mAppConstants = appConstants;
            mSerializer = new JsonSerializer();
            SettingsEntryUpdated = new ObservableProperty<SettingsEntry>();

            if (File.Exists(appConstants.TimeLoggerSettingsFilePath))
            {
                using (StreamReader file = File.OpenText(appConstants.TimeLoggerSettingsFilePath))
                {
                    mSettingsEntry = (SettingsEntry)mSerializer.Deserialize(file, typeof(SettingsEntry));
                }
            }
            else
            {
                if (!Directory.Exists(appConstants.TimeLoggerDirectoryPath))
                {
                    Directory.CreateDirectory(appConstants.TimeLoggerDirectoryPath);
                }
                mSettingsEntry = new SettingsEntry();
            }
        }

        public string UrlRoot => mSettingsEntry.UrlRoot;

        public string ApplicationIcon => mSettingsEntry.ApplicationIcon;

        public ObservableProperty<SettingsEntry> SettingsEntryUpdated
        {
            get;
        }

        public void Save(string urlRoot, string applicationIcon)
        {
            mSettingsEntry.UrlRoot = urlRoot;
            mSettingsEntry.ApplicationIcon = applicationIcon;
            mSettingsEntry.Timestamp = DateTime.Now;
            using (StreamWriter sw = new StreamWriter(mAppConstants.TimeLoggerSettingsFilePath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                mSerializer.Serialize(writer, mSettingsEntry);
            }
            SettingsEntryUpdated.Publish(mSettingsEntry);

        }
    }
}
