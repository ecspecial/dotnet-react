using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plorg.Model {
    public interface IElement {
        public Guid ID { get; }

        public Guid RID { get; }
    }
}
