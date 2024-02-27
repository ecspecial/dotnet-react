using Plorg.Model;

namespace PlorgWeb.WebDTO {

  public class PageWebDTO {
    public string name { get; set; } = "";
    public string content { get; set; } = "";
  }

  /// <summary>
  /// Элемент журнала
  /// </summary>
  public class JournalWebElement : IWebElement<JournalElement> {

    public string ID { get; set; } = "";
    public string RID { get; set; } = "";
    public string BID { get; set; } = "";

    public List<PageWebDTO> Pages { get; set; } = new List<PageWebDTO>();

    public JournalWebElement() { }

    public JournalWebElement(JournalElement element) {
      ToWebElement(element);
    }

    public JournalElement ToElement() {
      Guid id;
      if (ID == "")
        id = Guid.NewGuid();
      else
        id = Guid.Parse(ID);
      var element = new JournalElement(id, Guid.Parse(RID), Guid.Parse(BID));

      foreach (var pageDTO in Pages) {
        var page = new Page();
        page.name = pageDTO.name;
        page.content = pageDTO.content;
        element.Pages.Add(page);
      }

      return element;
    }

    public void ToWebElement(JournalElement element) {
      ID = element.ID.ToString();
      RID = element.RID.ToString();
      BID = element.BID.ToString();

      foreach (var page in element.Pages) {
        var pageDTO = new PageWebDTO();
        pageDTO.name = page.name;
        pageDTO.content = page.content;
        Pages.Add(pageDTO);
      }
    }
  }
}
