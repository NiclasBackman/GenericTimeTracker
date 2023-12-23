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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VolvoTimeLogger
{
    /// <summary>
    /// Interaction logic for TimeEntryUserControl.xaml
    /// </summary>
    public partial class TimeEntryUserControl : UserControl
    {
        public TimeEntryUserControl()
        {
            InitializeComponent();
            IVolvoTimeService volvoService = (IVolvoTimeService)App.Current.Services.GetService(typeof(IVolvoTimeService));
            ISelectionService selectionService = (ISelectionService)App.Current.Services.GetService(typeof(ISelectionService));

            DataContext = new TimeEntryUserControlViewModel(volvoService, selectionService);
        }
    }
}
