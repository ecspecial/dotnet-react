using Plorg.Model;

namespace PlorgWeb.WebDTO {
    public class RootWebElement: IWebElement<RootElement> {
        public string RID { get; set; } = "";
        public string name { get; set; } = "";
        public string password { get; set; } = "";

        public RootWebElement() { }

        public RootWebElement(RootElement element) {
            ToWebElement(element);
        }

        public RootElement ToElement() {
            var rootElement = new RootElement(Guid.Parse(RID));
            rootElement.Name = name;
            rootElement.Password = password;
            return rootElement;
        }

        public void ToWebElement(RootElement element) {
            RID = element.ID.ToString();
            name = element.Name;
            password = element.Password;
        }
    }
}
