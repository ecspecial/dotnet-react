using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plorg.Model {

    public enum RelativeType {
        Parent,
        Child
    }

    public class RelationElement : ITaskElement {
        public Guid ID { get; }
        public Guid RID { get; }
        public Guid BID { get; }

        public Guid Relative { get; set; }

        public RelativeType RelationType { get; set; }

        public RelationElement() { }

        public RelationElement(Guid id, Guid rid, Guid bid) {
            ID = id;
            RID = rid;
            BID = bid;
        }

        public override bool Equals(object? obj) {
            var item = obj as RelationElement;
            if (item == null) return false;

            return ID == item.ID && RID == item.RID && BID == item.BID && Relative == item.Relative && RelationType == item.RelationType;
        }
    }
}
