using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolvoTimeLogger
{
    public interface ISettingsService
    {
        string UrlRoot { get; }

        string ApplicationIcon { get; }

        void Save(string urlRoot, string applicationIcon);

        ObservableProperty<SettingsEntry> SettingsEntryUpdated { get; }

    }
}
