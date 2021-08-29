namespace Scipts.Skill.Runtime.EventInfo {
    public interface IEventInfo {
        EventType EventType { get; }
        void OnGUI();
    }
}