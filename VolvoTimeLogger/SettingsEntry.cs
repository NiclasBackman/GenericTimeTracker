using System;

namespace VolvoTimeLogger
{
    public class SettingsEntry
    {
        public DateTime Timestamp
        {
            get; set;
        }

        public string UrlRoot
        {
            get; set;
        }

        public string ApplicationIcon
        {
            get; set;
        }

        public string TimeAttributeName
        {
            get; set;
        }

        public string UrlRefAttributeName
        {
            get; set;
        }
    }
}
