using Plorg.Model;
using Plorg.Repo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlorgRepo.Repo.DTO {
    public class ChecklistElementDTO : IElementDTO<ChecklistElement> {

        public Guid ID { get; set; }
        public Guid RID { get; set; }
        public Guid BID { get; set; }

        public Guid ParentID { get; set; }

        public bool Checked { get; set; } = false;
        public bool ResetOnComplete { get; set; } = false;

        public ChecklistElement FromDTO() {
            var checklist = new ChecklistElement(ID, BID, RID);
            checklist.ParentID = ParentID;
            checklist.Checked = Checked;
            checklist.ResetOnComplete = ResetOnComplete;
            return checklist;
        }

        public void ToDTO(ChecklistElement element) {
            ID = element.ID;
            RID = element.RID;
            BID = element.BID;
            ParentID = element.ParentID;
            Checked = element.Checked;
            ResetOnComplete = element.ResetOnComplete;
        }
    }
}
