using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plorg.Model {

    public class TimeLimitElement : ITaskElement {
        public Guid ID { get; }
        public Guid RID { get; }
        public Guid BID { get; }

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

        public void Repeat(int count = 1, bool undo = false) {
            for (int i = 0; i < count; i++) {
                if (undo) {
                    Deadline = Deadline.AddYears(-repeatYears).AddMonths(-repeatMonths) - repeatSpan;
                } else {
                    Deadline = Deadline.AddYears(repeatYears).AddMonths(repeatMonths) + repeatSpan;
                }
                RepeatCount -= 1;

                if (RepeatCount == 0) {
                    AutoRepeat = false;
                    Repeated = false;
                }
            }
        }

        public TimeLimitElement() { }

        public TimeLimitElement(Guid id, Guid bid, Guid rid) {
            ID = id;
            BID = bid;
            RID = rid;
        }

        public override bool Equals(object? obj) {
            var item = obj as TimeLimitElement;
            if (item == null) return false;
            return ID == item.ID && BID == item.BID && RID == item.RID &&
                Deadline == item.Deadline && ExecutionTime == item.ExecutionTime &&
                WarningTime == item.WarningTime && repeatYears == item.repeatYears &&
                repeatMonths == item.repeatMonths && repeatSpan == item.repeatSpan &&
                AutoRepeat == item.AutoRepeat && RepeatCount == item.RepeatCount &&
                Active == item.Active && Repeated == item.Repeated;

        }
    }
}
