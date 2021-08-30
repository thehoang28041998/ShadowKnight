using Scipts.Exception;
using Scipts.Skill.Config.EventInfo;

namespace Scipts.Skill.Config.Model {
    public struct EventFactory {
        public BaseEventInfo GenerationEventInfo(EventType eventType) {
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

        /*public IEventLogic GenerationEventLogic(DefaultSkill parentSkill, BaseEvent @event) {
            EventType eventType = @event.EventInfo.EventType;
            switch (eventType) {
                case EventType.PlayAnimation:
                    return new AnimationEventLogic(parentSkill, @event);
                case EventType.Dash:
                    return new DashEventLogic(parentSkill.UniqueEntity, parentSkill, @event);
                case EventType.ModifierToTarget:
                    return new ModifierToTargetEventLogic(parentSkill.UniqueEntity, parentSkill, @event);
                case EventType.Vfx:
                    return new VfxEventLogic(parentSkill, @event);
                case EventType.Jump:
                    return new JumpEventLogic(parentSkill.UniqueEntity, parentSkill, @event);
                default:
                    throw new NotFoundException(typeof(IEventLogic), eventType.ToString());
            }
        }*/
    }
}