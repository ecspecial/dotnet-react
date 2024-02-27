using Plorg.Model;

namespace PlorgWeb.WebDTO {
    public class BaseWebElementDTO : IWebElementDTO<BaseWebElement> {
        public string RID { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        public BaseWebElement FromDTO() {
            var element = new BaseWebElement();
            element.RID = RID;
            element.Name = Name;    
            element.Description = Description;
            return element;
        }

        public void ToDTO(BaseWebElement element) {
            RID = element.RID;
            Name = element.Name;
            Description = element.Description;
        }
    }
}
