using Scipts.Exception;
using Scipts.Skill.Runtime.EventInfo;
using Scipts.Skill.Runtime.Trigger;
using UnityEngine;
using EventType = Scipts.Skill.Runtime.EventInfo.EventType;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Scipts.Skill.Config.Model {
    [System.Serializable]
    public class BaseEvent {
        public bool enable;
        public ITrigger trigger = new TimelineTrigger();
        public IEventInfo eventInfo = new AnimationEventInfo();

        public BaseEvent() {
        }

#if UNITY_EDITOR
        public bool foldEditor = true;
        
        public void OnGUI() {
            GUIStyle gs = new GUIStyle(EditorStyles.popup);
            GUIStyleState normal = new GUIStyleState {
                    background = gs.normal.background,
                    textColor = Color.green
            };
            gs.normal = normal;
            
            // todo: custom trigger
            string oldTrigger = trigger.TriggerType.ToString();
            TriggerType currentTrigger = trigger.TriggerType;
            currentTrigger = (TriggerType) EditorGUILayout.EnumPopup("Trigger: ", currentTrigger, gs);
            if (oldTrigger != currentTrigger.ToString()) {
                trigger = GenerationTrigger(currentTrigger);
            }
            trigger.OnGUI();
            
            // todo: custom event info
            string oldEventInfo = eventInfo.EventType.ToString();
            EventType currentEvent = eventInfo.EventType;
            currentEvent = (EventType) EditorGUILayout.EnumPopup("Action: ", currentEvent, gs);
            if (oldEventInfo != currentEvent.ToString()) {
                eventInfo = GenerationEventInfo(currentEvent);
            }
            eventInfo.OnGUI();
        }
#endif

        public IEventInfo GenerationEventInfo(EventType eventType) {
            switch (eventType) {
                case EventType.PlayAnimation:
                    return new AnimationEventInfo();
                default:
                    throw new NotFoundException(typeof(IEventInfo), eventType.ToString());
            }
        }

        public ITrigger GenerationTrigger(TriggerType type) {
            switch (type) {
                case TriggerType.Frame:
                    return new TimelineTrigger();
                case TriggerType.Event:
                    return new EventTrigger();
            }

            throw new NotFoundException(typeof(ITrigger), "with trigger type " + type);
        }
    }
}