using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VolvoTimeLogger
{
    public class TimeEntriesToSummaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ReadOnlyObservableCollection<object> items = (ReadOnlyObservableCollection<object>)value;
            if(items == null)
                return null;

            float sum = 0.0F;
            foreach(var entry in items)
            {
                var e = entry as TimeEntry;
                sum += e.NoOfHours;
            }
            return sum;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
