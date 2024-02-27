namespace PlorgWeb.WebDTO {
    /// <summary>
    /// Элементы задач
    /// </summary>
    public class TaskElements {
        /// <summary>
        /// Базовые элементы
        /// </summary>
        public List<BaseWebElement> baseElements { get; set; } = new List<BaseWebElement>();
        /// <summary>
        /// Журналы
        /// </summary>
        public List<JournalWebElement> journals { get; set; } = new List<JournalWebElement>();
        /// <summary>
        /// Отношения
        /// </summary>
        public List<RelationWebElement> relations { get; set; } = new List<RelationWebElement>();
        /// <summary>
        /// Чеклисты
        /// </summary>
        public List<ChecklistWebElement> checklists { get; set; } = new List<ChecklistWebElement>();
        /// <summary>
        /// Временные ограничения
        /// </summary>
        public List<TimeLimitWebElement> timeLimits { get; set; } = new List<TimeLimitWebElement>();
    }
}
