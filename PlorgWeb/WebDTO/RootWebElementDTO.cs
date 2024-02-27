using Plorg.Model;

namespace PlorgWeb.WebDTO {
    public class RootWebElementDTO: IWebElementDTO<RootWebElement> {
        public string? name { get; set; } = "";
        public string? password { get; set; } = "";

        public RootWebElementDTO() { }

        public RootWebElement FromDTO() {
            var element = new RootWebElement();
            element.name = name == null ? "" : name;
            element.password = password == null ? "" : password;
            return element;
        }

        public void ToDTO(RootWebElement element) {
            name = element.name;
            password = element.password;
        }
    }
}
