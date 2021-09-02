using System;
using Scipts.Helper;

namespace Scipts.Skills.Core.Event.Trigger {
    [Serializable]
    public class EventTrigger : BaseTrigger {
        public int id { get; private set; }

        public EventTrigger() : base(TriggerType.Event) {
        }

#if UNITY_EDITOR
        protected override void OnDetailGUI() {
            id = EditorHelper.Int("Id", id);
        }
#endif
    }
}