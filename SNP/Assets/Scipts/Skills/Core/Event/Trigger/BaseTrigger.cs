using System;
using Scipts.Helper;
using UnityEngine;

namespace Scipts.Skills.Core.Event.Trigger {
    [Serializable]
    public abstract class BaseTrigger {
        [SerializeField] protected string triggerType { get; private set; }

        protected BaseTrigger(TriggerType triggerType) {
            this.triggerType = triggerType.ToString();
        }

        public TriggerType TriggerType {
            get => (TriggerType) Enum.Parse(typeof(TriggerType), triggerType);
        }

#if UNITY_EDITOR
        internal bool triggerChanged;

        protected abstract void OnDetailGUI();

        public void OnGUI() {
            GUIStyle gs = new GUIStyle(EditorHelper.Popup);
            GUIStyleState normal = new GUIStyleState {
                background = gs.normal.background,
                textColor = Color.green
            };
            gs.normal = normal;

            triggerChanged = false;
            string old = triggerType;
            TriggerType current = TriggerType;
            current = (TriggerType) EditorHelper.EnumPopup("Trigger: ", current, gs);
            if (old != current.ToString()) {
                triggerType = current.ToString();
                triggerChanged = true;
            }

            OnDetailGUI();
        }
#endif
    }
    
    public enum TriggerType {
        Frame,
        Event
    }
}