using Scipts.Skill.Config.Model;
using Scipts.Skill.Runtime.EventInfo;
using Scipts.Skill.Runtime.Trigger;

namespace Scipts.Skill.Runtime.Model {
    public struct BaseEventData {
        public ITrigger trigger;
        public IEventInfo eventInfo;
        
        public BaseEventData(BaseEvent baseEvent) {
            this.trigger = baseEvent.trigger;
            this.eventInfo = baseEvent.eventInfo;
        }
    }
}