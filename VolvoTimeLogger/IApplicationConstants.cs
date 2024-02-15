namespace VolvoTimeLogger
{
    public interface IApplicationConstants
    {
        string TimeLoggerStorageFilePath { get; }

        string TimeLoggerSettingsFilePath { get; }

        string TimeLoggerDirectoryPath { get; }
    }
}