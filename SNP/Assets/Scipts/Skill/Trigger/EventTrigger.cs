#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Skill.Trigger {
    [System.Serializable]
    public struct EventTrigger : ITrigger {
        public int eventId;

        public TriggerType TriggerType {
            get => TriggerType.Event;
        }

#if UNITY_EDITOR
        public void OnGUI() {
            eventId = EditorGUILayout.IntField($"EventId", eventId);
        }
#endif
    }
}