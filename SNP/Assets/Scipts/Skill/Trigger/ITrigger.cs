namespace Skill.Trigger {
    public interface ITrigger {
        TriggerType TriggerType { get; }

#if UNITY_EDITOR
        void OnGUI();
#endif
    }
}