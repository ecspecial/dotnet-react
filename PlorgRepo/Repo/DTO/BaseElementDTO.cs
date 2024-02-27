using Plorg.Model;
using Plorg.Repo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlorgRepo.Repo.DTO {
    public class BaseElementDTO : IElementDTO<BaseElement> {
        public Guid BID { get; set; }

        public Guid RID { get; set; }

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        public BaseElement FromDTO() {
            return new BaseElement(BID, RID, Name, Description);
        }

        public void ToDTO(BaseElement element) {
            BID = element.BID;
            RID = element.RID;
            Name = element.Name;
            Description = element.Description;
        }
    }
}
