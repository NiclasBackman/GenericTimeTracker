using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace VolvoTimeLogger
{
    public class AutoHideMenuItem : MenuItem
    {
        DispatcherTimer timer;

        private Int32 _autoHideDelay;
        public Int32 AutoHideDelay
        {
            get
            {
                return _autoHideDelay;
            }
            set
            {
                _autoHideDelay = value;
                timer.Interval = TimeSpan.FromSeconds(AutoHideDelay);
            }
        }

        public AutoHideMenuItem()
        {
            MouseMove += new MouseEventHandler(AutoHideMenuItem_MouseMove);
            ContextMenuOpening += new ContextMenuEventHandler(AutoHideMenuItem_ContextMenuOpening);
            SubmenuClosed += new RoutedEventHandler(AutoHideMenuItem_SubmenuClosed);

            timer = new DispatcherTimer(DispatcherPriority.Normal, Dispatcher);
            timer.Interval = TimeSpan.FromSeconds(AutoHideDelay);
            timer.Stop();

            timer.Tick += new EventHandler(timer_Tick);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (IsSubmenuOpen && !IsMouseOver)
            {
                IsSubmenuOpen = false;
            }
        }

        void AutoHideMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            timer.Stop();
            timer.Start();
        }

        void AutoHideMenuItem_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            timer.Start();
        }

        void AutoHideMenuItem_SubmenuClosed(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }
    }
}
