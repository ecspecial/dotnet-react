using Plorg.Model;

namespace PlorgWeb.WebDTO {
    public class JournalWebElementDTO : IWebElementDTO<JournalWebElement> {
        public string RID { get; set; } = "";
        public string BID { get; set; } = "";

        public List<PageWebDTO> Pages { get; set; } = new List<PageWebDTO>();


        public JournalWebElement FromDTO() {
            var element = new JournalWebElement();

            element.BID = BID;
            element.RID = RID;
            element.Pages = Pages;
            
            return element;
        }

        public void ToDTO(JournalWebElement element) {
            RID = element.RID;
            BID = element.BID;

            Pages = element.Pages;
        }
    }
}
