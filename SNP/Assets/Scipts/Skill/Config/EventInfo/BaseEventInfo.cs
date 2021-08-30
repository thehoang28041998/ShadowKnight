using System;
using Scipts.Helper;
using UnityEngine;

namespace Scipts.Skill.Config.EventInfo {
    [Serializable]
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

        public void OnGUI() {
            GUIStyle gs = new GUIStyle(EditorHelper.Popup);
            GUIStyleState normal = new GUIStyleState {
                background = gs.normal.background,
                textColor = Color.cyan
            };
            gs.normal = normal;

            editorChanged = false;
            string old = actionType;
            EventType current = EventType;
            current = (EventType) EditorHelper.EnumPopup("Action: ", current, gs);
            if (old != current.ToString()) {
                actionType = current.ToString();
                editorChanged = true;
            }

            OnDetailGUI();
        }

        protected abstract void OnDetailGUI();
#endif
    }

    public enum EventType {
        CastProjectile,
        PlayAnimation,
        Dash,
        ModifierToTarget,
        Vfx,
        Jump
    }
}