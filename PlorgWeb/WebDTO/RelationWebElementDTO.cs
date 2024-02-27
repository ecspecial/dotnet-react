using Plorg.Model;

namespace PlorgWeb.WebDTO {
    public class RelationWebElementDTO : IWebElementDTO<RelationWebElement> {

        public string RID { get; set; } = "";
        public string BID { get; set; } = "";

        public string Relative { get; set; } = ""; 

        public RelativeTypeDTO RelationType { get; set; }

        public RelationWebElement FromDTO() {
            var element = new RelationWebElement();
            element.BID = BID;
            element.RID = RID;
            element.RelationType = RelationType;
            element.Relative = Relative;

            return element;
        }

        public void ToDTO(RelationWebElement element) {
            RID = element.RID;
            BID = element.BID;
            RelationType = element.RelationType;
            Relative = element.Relative;
        }
    }
}
