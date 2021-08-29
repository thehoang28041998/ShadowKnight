#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Scipts.Skill.Config.Trigger {
    [System.Serializable]
    public class EventTrigger : BaseTrigger {
        public int eventId;
      
        public EventTrigger() : base(TriggerType.Event) {
        }

#if UNITY_EDITOR
        protected override void OnDetailGUI() {
            eventId = EditorGUILayout.IntField($"EventId", eventId);
        }
#endif
    }
}