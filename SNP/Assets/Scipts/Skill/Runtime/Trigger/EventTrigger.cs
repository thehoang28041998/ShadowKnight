#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Scipts.Skill.Runtime.Trigger {
    [System.Serializable]
    public struct EventTrigger : ITrigger {
        public int eventId;


#if UNITY_EDITOR
        public void OnGUI() {
            eventId = EditorGUILayout.IntField($"EventId", eventId);
        }
#endif
       
        public TriggerType TriggerType {
            get => TriggerType.Event;
        }
    }
}