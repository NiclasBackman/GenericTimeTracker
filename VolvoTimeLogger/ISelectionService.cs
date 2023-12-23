using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolvoTimeLogger
{
    public interface ISelectionService
    {
        ObservableProperty<Guid> SelectionChanged
        {
            get;
        }

        void Select(Guid id);
    }
}
