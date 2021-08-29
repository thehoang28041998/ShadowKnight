using Exception;
using Skill.Config.EventInfo;
using Skill.Config.Trigger;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Skill.Config.Model {
    [System.Serializable]
    public class BaseEvent {
        public bool enable;
        public BaseTrigger trigger = new TimelineTrigger();
        public BaseEventInfo eventInfo = new AnimationEventInfo();

        public BaseEvent() {
        }

#if UNITY_EDITOR
        public bool foldEditor = true;

        internal BaseEvent(BaseTrigger trigger) {
            this.trigger = trigger;
        }

        internal BaseEvent(BaseEventInfo eventInfo) {
            this.eventInfo = eventInfo;
        }

        public void OnGUI() {
            trigger.OnGUI();
            if (trigger.triggerChanged) trigger = GenerationTrigger(trigger.TriggerType);

            eventInfo.OnGUI();
            if (eventInfo.editorChanged) eventInfo = GenerationEventInfo(eventInfo.EventType);
        }
#endif

        public BaseEventInfo GenerationEventInfo(EventType eventType) {
            switch (eventType) {
                case EventType.PlayAnimation:
                    return new AnimationEventInfo();
                default:
                    throw new NotFoundException(typeof(BaseEventInfo), eventType.ToString());
            }
        }

        public BaseTrigger GenerationTrigger(TriggerType type) {
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