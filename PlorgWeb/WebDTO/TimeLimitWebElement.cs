using Plorg.Model;

namespace PlorgWeb.WebDTO {
  public class TimeLimitWebElement : IWebElement<TimeLimitElement> {

    public string ID { get; set; } = "";
    public string RID { get; set; } = "";
    public string BID { get; set; } = "";

    public DateTime Deadline { get; set; } = DateTime.MinValue;
    public string ExecutionTime { get; set; } = "";
    public string WarningTime { get; set; } = "";

    public int repeatYears = 0;
    public int repeatMonths = 0;
    public string RepeatSpan { get; set; } = "";

    public bool AutoRepeat { get; set; } = false;

    public int RepeatCount { get; set; } = 0;

    public bool Active { get; set; } = false;

    public bool Repeated { get; set; } = false;

    public TimeLimitWebElement() { }

    public TimeLimitWebElement(TimeLimitElement element) {
      ToWebElement(element);
    }

    public TimeLimitElement ToElement() {
      Guid id;
      if (ID == "")
        id = Guid.NewGuid();
      else
        id = Guid.Parse(ID);
      var element = new TimeLimitElement(id, Guid.Parse(BID), Guid.Parse(RID));
      element.Deadline = Deadline;
      element.ExecutionTime = TimeSpan.Parse(ExecutionTime);
      element.WarningTime = TimeSpan.Parse(WarningTime);

      element.repeatYears = repeatYears;
      element.repeatMonths = repeatMonths;
      element.repeatSpan = TimeSpan.Parse(RepeatSpan);

      element.AutoRepeat = AutoRepeat;
      element.RepeatCount = RepeatCount;
      element.Active = Active;
      element.Repeated = Repeated;

      return element;
    }

    public void ToWebElement(TimeLimitElement element) {
      ID = element.ID.ToString();
      BID = element.BID.ToString();
      RID = element.RID.ToString();

      Deadline = element.Deadline;
      ExecutionTime = element.ExecutionTime.ToString();
      WarningTime = element.WarningTime.ToString();

      repeatYears = element.repeatYears;
      repeatMonths = element.repeatMonths;
      RepeatSpan = element.repeatSpan.ToString();

      AutoRepeat = element.AutoRepeat;
      RepeatCount = element.RepeatCount;
      Active = element.Active;
      Repeated = element.Repeated;
    }
  }
}
