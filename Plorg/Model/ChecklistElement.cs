using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plorg.Model {
    public class ChecklistElement : ITaskElement {
        public Guid ID { get; }
        public Guid RID { get; }
        public Guid BID { get; }


        public Guid ParentID { get; set; }

        public bool Checked { get; set; } = false;

        public bool ResetOnComplete { get; set; } = true;

        public ChecklistElement() { }

        public ChecklistElement(Guid id, Guid bid, Guid rid) {
            ID = id;
            BID = bid;
            RID = rid;
        }

        public override bool Equals(object? obj) {
            var item = obj as ChecklistElement;
            if (item == null) return false;
            return item.ID == ID && item.BID == BID &&
                   item.RID == RID && item.ParentID == ParentID && 
                   item.Checked == Checked && item.ResetOnComplete == ResetOnComplete;
        }
    }
}
