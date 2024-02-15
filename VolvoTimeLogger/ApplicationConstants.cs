using System;
using System.IO;

namespace VolvoTimeLogger
{
    public class ApplicationConstants : IApplicationConstants
    {
        private readonly string mFilePath =
            Path.Combine(new []
            {
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ZalcinSoft",
                "GenericTimeLogger",
                "timelog.json" 
            });

        private readonly string mFileSettingsPath =
            Path.Combine(new[]
            {
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ZalcinSoft",
                "GenericTimeLogger",
                "timelogsettings.json"
            });

        private readonly string mDirectoryPath =
            Path.Combine(new[]
            {
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ZalcinSoft",
                "GenericTimeLogger"
            });

        public string TimeLoggerStorageFilePath => mFilePath;

        public string TimeLoggerSettingsFilePath => mFileSettingsPath;

        public string TimeLoggerDirectoryPath => mDirectoryPath;
    }
}
