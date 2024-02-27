using Plorg.Model;
using Plorg.Repo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlorgRepo.Repo.DTO {
    public class TimeLimitElementDTO : IElementDTO<TimeLimitElement> {
        public Guid ID { get; set; }
        public Guid RID { get; set; }
        public Guid BID { get; set; }

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

        public TimeLimitElement FromDTO() {
            var timeLimitElement = new TimeLimitElement(ID, BID, RID);

            timeLimitElement.Deadline = Deadline;
            timeLimitElement.ExecutionTime = ExecutionTime;
            timeLimitElement.WarningTime = WarningTime;
            timeLimitElement.repeatYears = repeatYears;
            timeLimitElement.repeatMonths = repeatMonths;
            timeLimitElement.repeatSpan = repeatSpan;
            timeLimitElement.AutoRepeat = AutoRepeat;
            timeLimitElement.RepeatCount = RepeatCount;
            timeLimitElement.Active = Active;
            timeLimitElement.Repeated = Repeated;

            return timeLimitElement;
        }

        public void ToDTO(TimeLimitElement element) {
            ID = element.ID;
            RID = element.RID;
            BID = element.BID;

            Deadline = element.Deadline;
            ExecutionTime = element.ExecutionTime;
            WarningTime = element.WarningTime;
            repeatYears = element.repeatYears;
            repeatMonths = element.repeatMonths;
            repeatSpan = element.repeatSpan;
            AutoRepeat = element.AutoRepeat;
            RepeatCount = element.RepeatCount;
            Active = element.Active;
            Repeated = element.Repeated;
        }
    }
}
