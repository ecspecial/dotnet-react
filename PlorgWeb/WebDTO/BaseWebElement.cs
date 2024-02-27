using Plorg.Model;

namespace PlorgWeb.WebDTO {
    /// <summary>
    /// Базовый элемент задачи
    /// </summary>
    public class BaseWebElement: IWebElement<BaseElement> {
        public string BID { get; set; } = "";

        public string RID { get; set; } = "";

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";


        public BaseWebElement() { }

        public BaseWebElement(BaseElement element) {
            ToWebElement(element);
        }

        public BaseElement ToElement() {
            return new BaseElement(Guid.Parse(BID), Guid.Parse(RID), Name, Description);
        }

        public void ToWebElement(BaseElement element) {
            BID = element.BID.ToString();
            RID = element.RID.ToString();
            Name = element.Name;
            Description = element.Description;
        }
    }
}
