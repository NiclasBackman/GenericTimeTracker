using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VolvoTimeLogger
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window, ISettingsWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();
            ISettingsService settingsService = (ISettingsService)App.Current.Services.GetService(typeof(ISettingsService));

            this.DataContext = new SettingsWindowViewModel(this, settingsService);
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
        }        
    }
}
