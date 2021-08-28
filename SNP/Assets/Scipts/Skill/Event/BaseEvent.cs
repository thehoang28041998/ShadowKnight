using Skill.Trigger;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Skill.Event {
    [System.Serializable]
    public struct BaseEvent {
        public bool enable;
        public ITrigger trigger;

#if UNITY_EDITOR

        private TriggerType triggerTypeEditor;

        public void OnGUI() {
            enable = EditorGUILayout.Toggle("Enable", enable);
            if (trigger == null) {
                trigger = new TimelineTrigger();
            }

            triggerTypeEditor = trigger.TriggerType;


            GUILayout.BeginVertical("window");
            triggerTypeEditor = (TriggerType) EditorGUILayout.EnumPopup("Trigger", triggerTypeEditor);
            trigger.OnGUI();
            GUILayout.EndVertical();

            if (triggerTypeEditor != trigger.TriggerType) {
                switch (triggerTypeEditor) {
                    case TriggerType.Event:
                        trigger = new EventTrigger();
                        break;
                    case TriggerType.Frame:
                        trigger = new TimelineTrigger();
                        break;
                }
            }
        }
#endif
    }
}