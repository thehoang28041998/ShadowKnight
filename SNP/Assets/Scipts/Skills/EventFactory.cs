using Scipts.Exception;
using Scipts.Skills.Combat.Event.Info;
using Scipts.Skills.Core.Event.Info;

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
    }
}