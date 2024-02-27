using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plorg.Model {
    public struct ElementNode {
        IElement root;

        List<IElement> children;
    }

    public interface ITaskElement: IElement {
        public Guid BID { get; }
    }
}
