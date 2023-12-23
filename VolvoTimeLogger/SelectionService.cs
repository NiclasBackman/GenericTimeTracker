using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolvoTimeLogger
{
    class SelectionService : ISelectionService
    {
        public SelectionService()
        {
            SelectionChanged = new ObservableProperty<Guid>();
        }

        public ObservableProperty<Guid> SelectionChanged
        {
            get;
        }

        public void Select(Guid id)
        {
            SelectionChanged.Publish(id);
        }
    }
}
