namespace PlorgWeb.WebDTO {
    /// <summary>
    /// Элемент временных ограничений
    /// </summary>
    public class TimeLimitWebElementDTO : IWebElementDTO<TimeLimitWebElement> {

        public string RID { get; set; } = "";
        public string BID { get; set; } = "";

        public DateTime Deadline { get; set; } = DateTime.MinValue;
        public TimeSpan ExecutionTime { get; set; } = TimeSpan.Zero;
        public TimeSpan WarningTime { get; set; } = TimeSpan.Zero;

        public int repeatYears = 0;
        public int repeatMonths = 0;
        public TimeSpan repeatSpan = TimeSpan.Zero;

        public bool AutoRepeat { get; set; } = false;

        public int RepeatCount { get; set; } = 0;

        public bool Active { get; set; } = false;

        public bool Repeated { get; set; } = false;

        public TimeLimitWebElement FromDTO() {

            var element = new TimeLimitWebElement();

            element.BID = BID;
            element.RID = RID;

            element.Deadline = Deadline;
            element.ExecutionTime = ExecutionTime.ToString();
            element.WarningTime = WarningTime.ToString();

            element.repeatYears = repeatYears;
            element.repeatMonths = repeatMonths;
            element.RepeatSpan = repeatSpan.ToString();

            element.AutoRepeat = AutoRepeat;
            element.RepeatCount = RepeatCount;
            element.Active = Active;
            element.Repeated = Repeated;

            return element;
        }

        public void ToDTO(TimeLimitWebElement element) {
            BID = element.BID;
            RID = element.RID;

            Deadline = element.Deadline;
            ExecutionTime = TimeSpan.Parse(element.ExecutionTime);
            WarningTime = TimeSpan.Parse(element.WarningTime);

            repeatYears = element.repeatYears;
            repeatMonths = element.repeatMonths;
            repeatSpan = TimeSpan.Parse(element.RepeatSpan);

            AutoRepeat = element.AutoRepeat;
            RepeatCount = element.RepeatCount;
            Active = element.Active;
            Repeated = element.Repeated;
        }
    }
}
