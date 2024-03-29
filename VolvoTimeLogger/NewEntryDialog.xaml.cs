﻿using System;
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
    /// Interaction logic for NewEntryDialog.xaml
    /// </summary>
    public partial class NewEntryDialog : Window
    {
        public NewEntryDialog(IVolvoTimeService service, ISettingsService mSettingsService)
        {
            InitializeComponent();
            DataContext = new NewEntryDialogViewModel(service, this, mSettingsService);
        }
    }
}
