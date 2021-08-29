using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Skill.Config.EventInfo {
    [System.Serializable]
    public abstract class BaseEventInfo {
        [SerializeField] protected string actionType;

        protected BaseEventInfo(EventType eventType) {
            this.actionType = eventType.ToString();
        }

        public EventType EventType {
            get => (EventType) Enum.Parse(typeof(EventType), actionType);
        }

#if UNITY_EDITOR
        public bool editorChanged;

        protected abstract void OnDetailGUI();

        public void OnGUI() {
            GUIStyle gs = new GUIStyle(EditorStyles.popup);
            GUIStyleState normal = new GUIStyleState {
                    background = gs.normal.background,
                    textColor = Color.cyan
            };
            gs.normal = normal;

            editorChanged = false;
            string old = actionType;
            EventType current = EventType;
            current = (EventType) EditorGUILayout.EnumPopup("Action: ", current, gs);
            if (old != current.ToString()) {
                actionType = current.ToString();
                editorChanged = true;
            }

            OnDetailGUI();
        }
#endif
    }
}