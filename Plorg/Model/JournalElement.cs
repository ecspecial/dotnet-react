using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Plorg.Model {
    public struct Page {
        public string name;
        public string content;
    }

    public class JournalElement : ITaskElement {
        public Guid ID { get; }
        public Guid RID { get; }
        public Guid BID { get; }

        public List<Page> Pages { get; set; } = new List<Page>();

        public JournalElement() { }

        public JournalElement(Guid id, Guid rid, Guid bid) {
            ID = id;
            RID = rid;
            BID = bid;
        }

        public override bool Equals(object? obj) {
            var item = obj as JournalElement;
            if (item == null) return false;
            return item.ID == ID && item.BID == BID && item.RID == RID && item.Pages.SequenceEqual(Pages);
        }
    }
}
