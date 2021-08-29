namespace Scipts.Skill.Runtime.Trigger {
    public interface ITrigger {
        TriggerType TriggerType { get; }

#if UNITY_EDITOR
        void OnGUI();
#endif
    }
}