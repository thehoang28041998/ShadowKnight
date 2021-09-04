using System;
using Scipts.Exception;
using Scipts.Skills.Combat.Event.Info;
using Scipts.Skills.Core.Event.Info;
using Scipts.Skills.Core.Event.Trigger;
using UnityEngine;

namespace Scipts.Skills.Core.Event
{
    [Serializable]
    public class BaseEvent
    {
        public bool enable = true;
        public BaseTrigger trigger { get; private set; } = new TimelineTrigger();
        public BaseEventInfo eventInfo { get; private set; } = new AnimationEventInfo();

        public BaseEvent() {
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
                eventInfo = EventFactory.GenerationEventInfo(eventInfo.EventType);
            }
        }
#endif

        BaseTrigger GetTrigger(TriggerType type)
        {
            switch (type)
            {
                case TriggerType.Frame:
                    return new TimelineTrigger();
                case TriggerType.Event:
                    return new EventTrigger();
            }

            throw new NotFoundException(typeof(BaseTrigger), "with trigger type " + type);
        }
    }
}