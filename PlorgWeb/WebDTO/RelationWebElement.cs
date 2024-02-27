using Plorg.Model;

namespace PlorgWeb.WebDTO {

  public enum RelativeTypeDTO {
    Parent,
    Child
  }

  /// <summary>
  /// Элемент отношений
  /// </summary>
  public class RelationWebElement : IWebElement<RelationElement> {
    public string ID { get; set; } = "";
    public string RID { get; set; } = "";
    public string BID { get; set; } = "";

    public string Relative { get; set; } = "";

    public RelativeTypeDTO RelationType { get; set; }

    public RelationWebElement() { }

    public RelationWebElement(RelationElement element) {
      ToWebElement(element);
    }

    public RelationElement ToElement() {
      Guid id;
      if (ID == "")
        id = Guid.NewGuid();
      else
        id = Guid.Parse(ID);
      var element = new RelationElement(id, Guid.Parse(RID), Guid.Parse(BID));

      element.Relative = Guid.Parse(Relative);
      element.RelationType = (RelativeType)RelationType;

      return element;
    }

    public void ToWebElement(RelationElement element) {
      ID = element.ID.ToString();
      RID = element.RID.ToString();
      BID = element.BID.ToString();

      Relative = element.Relative.ToString();

      RelationType = (RelativeTypeDTO)element.RelationType;
    }
  }
}
