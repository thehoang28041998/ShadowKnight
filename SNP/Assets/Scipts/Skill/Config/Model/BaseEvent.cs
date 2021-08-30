using System;
using Scipts.Exception;
using Scipts.Skill.Config.EventInfo;
using Scipts.Skill.Config.Trigger;
using UnityEngine;

namespace Scipts.Skill.Config.Model {
    [Serializable]
    public class BaseEvent {
        [SerializeField] private bool enable = true;
        [SerializeField] private BaseTrigger trigger = new TimelineTrigger();
        [SerializeField] private BaseEventInfo eventInfo = new AnimationEventInfo();

        public BaseEvent() {
        }


        public bool Enable {
            get => enable;
            set => enable = value;
        }

        public BaseTrigger Trigger {
            get => trigger;
        }

        public BaseEventInfo EventInfo {
            get => eventInfo;
        }

#if UNITY_EDITOR
        internal BaseEvent(BaseTrigger trigger) {
            this.trigger = trigger;
        }

        internal BaseEvent(BaseEventInfo eventInfo) {
            this.eventInfo = eventInfo;
        }

        [SerializeField] public bool fold = true;

        public void OnGUI() {
            trigger.OnGUI();
            if (trigger.triggerChanged)
                trigger = GetTrigger(trigger.TriggerType);

            eventInfo.OnGUI();
            if (eventInfo.editorChanged) {
                eventInfo = new EventFactory().GenerationEventInfo(eventInfo.EventType);
            }
        }
#endif

        BaseTrigger GetTrigger(TriggerType type) {
            switch (type) {
                case TriggerType.Frame:
                    return new TimelineTrigger();
                case TriggerType.Event:
                    return new EventTrigger();
            }

            throw new NotFoundException(typeof(BaseTrigger), "with trigger type " + type);
        }
    }
}