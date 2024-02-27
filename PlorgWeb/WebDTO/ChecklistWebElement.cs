using Plorg.Model;

namespace PlorgWeb.WebDTO {
  /// <summary>
  /// Элемент чеклиста
  /// </summary>
  public class ChecklistWebElement : IWebElement<ChecklistElement> {

    public string ID { get; set; } = "";
    public string RID { get; set; } = "";
    public string BID { get; set; } = "";

    public bool Checked { get; set; } = false;
    public bool ResetOnComplete { get; set; } = false;

    public string ParentID { get; set; } = "";

    public ChecklistWebElement() { }

    public ChecklistWebElement(ChecklistElement element) {
      ToWebElement(element);
    }

    public ChecklistElement ToElement() {
      Guid id;
      if (ID == "")
        id = Guid.NewGuid();
      else
        id = Guid.Parse(ID);
      var element = new ChecklistElement(id, Guid.Parse(BID), Guid.Parse(RID));
      element.Checked = Checked;
      element.ResetOnComplete = ResetOnComplete;
      element.ParentID = Guid.Parse(ParentID);
      return element;
    }

    public void ToWebElement(ChecklistElement element) {
      ID = element.ID.ToString();
      BID = element.BID.ToString();
      RID = element.RID.ToString();

      ParentID = element.ParentID.ToString();

      Checked = element.Checked;
      ResetOnComplete = element.ResetOnComplete;
    }
  }
}
