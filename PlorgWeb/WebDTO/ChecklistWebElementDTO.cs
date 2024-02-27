using Plorg.Model;

namespace PlorgWeb.WebDTO {
    public class ChecklistWebElementDTO : IWebElementDTO<ChecklistWebElement> {
        public string RID { get; set; } = "";
        public string BID { get; set; } = "";

        public bool Checked { get; set; } = false;
        public bool ResetOnComplete { get; set; } = false;

        public string ParentID { get; set; } = "";


        public ChecklistWebElement FromDTO() {
            var element = new ChecklistWebElement();
            element.BID = BID;
            element.RID = RID;
            element.Checked = Checked;
            element.ResetOnComplete = ResetOnComplete;
            element.ParentID = ParentID;
            return element;
        }

        public void ToDTO(ChecklistWebElement element) {
            BID = element.BID;
            RID = element.RID;

            ParentID = element.ParentID;

            Checked = element.Checked;
            ResetOnComplete = element.ResetOnComplete;
        }
    }
}
