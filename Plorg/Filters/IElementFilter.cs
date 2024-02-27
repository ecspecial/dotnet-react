using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plorg.Filters {
    public interface IElementFilter<T> {
        IEnumerable<T> Filter(IEnumerable<T> elements);
    }
}
