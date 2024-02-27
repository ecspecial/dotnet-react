using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Plorg.Model {

    public class RootElement : IElement {

        protected Guid id;
        public Guid ID {
            get {
                return id;
            }
        }

        public Guid RID {
            get {
                return id;
            }
        }

        public string Name { get; set; } = "";

        public string Password { get; set; } = "";

        public RootElement() {
            this.id = Guid.NewGuid();
        }

        public RootElement(Guid id, string name = "", string? password = null) {
            this.id = id;
            Name = name;
            if (password != null) {
                Password = password;
            }
        }

        public override bool Equals(object? obj) {
            var item = obj as RootElement;

            if (item == null) { return false; }

            return item.RID == RID && item.Name == Name && item.Password == Password;
        }
    }
}
