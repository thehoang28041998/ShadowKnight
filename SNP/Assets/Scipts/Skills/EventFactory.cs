using Scipts.Exception;
using Scipts.Skills.Combat.Event.Info;
using Scipts.Skills.Combat.Model;
using Scipts.Skills.Core.Event;
using Scipts.Skills.Core.Event.Info;
using Scipts.Skills.Core.Event.Logic;

namespace Scipts.Skills {
    public static class EventFactory {
        public static BaseEventInfo GenerationEventInfo(EventType eventType) {
            switch (eventType) {
                case EventType.PlayAnimation:
                    return new AnimationEventInfo();
                /*case EventType.CastProjectile:
                    return new CastProjectileEventInfo();
                case EventType.Dash:
                    return new DashEventInfo();
                case EventType.ModifierToTarget:
                    return new ModifierToTargetEventInfo();
                case EventType.Vfx:
                    return new VfxEventInfo();
                case EventType.Jump:
                    return new JumpEventInfo();*/
                default:
                    throw new NotFoundException(typeof(BaseEventInfo), eventType.ToString());
            }
        }
        
        public static IEventLogic GenerationEventLogic(DefaultSkill parentSkill, BaseEvent @event) {
            EventType eventType = @event.eventInfo.EventType;
            switch (eventType) {
                /*case EventType.PlayAnimation:
                    return new AnimationEventLogic(parentSkill, @event);
                case EventType.Dash:
                    return new DashEventLogic(parentSkill.UniqueEntity, parentSkill, @event);
                case EventType.ModifierToTarget:
                    return new ModifierToTargetEventLogic(parentSkill.UniqueEntity, parentSkill, @event);
                case EventType.Vfx:
                    return new VfxEventLogic(parentSkill, @event);
                case EventType.Jump:
                    return new JumpEventLogic(parentSkill.UniqueEntity, parentSkill, @event);*/
                default:
                    throw new NotFoundException(typeof(IEventLogic), eventType.ToString());
            }
        }
    }
}