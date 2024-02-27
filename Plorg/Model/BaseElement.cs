using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plorg.Model {
    public class BaseElement : ITaskElement {

        protected Guid id;

        public Guid ID {
            get {
                return id;
            }
        }

        public Guid BID { get => id;}

        public Guid RID { get; }

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        public BaseElement() { }

        public BaseElement(Guid bid, Guid rid, string name = "", string description = "") {
            this.id = bid;
            this.RID = rid;
            Name = name;
            Description = description;
        }

        public override bool Equals(object? obj) {
            var item = obj as BaseElement;
            if (item == null) return false;
            return BID == item.BID && RID == item.RID && Name == item.Name && Description == item.Description;
        }
    }
}
