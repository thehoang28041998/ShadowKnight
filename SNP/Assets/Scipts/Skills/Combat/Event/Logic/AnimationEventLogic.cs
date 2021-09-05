using Scipts.EntityComponentSystem;
using Scipts.Skills.Combat.Event.Info;
using Scipts.Skills.Combat.Model;
using Scipts.Skills.Core.Event;
using Scipts.Skills.Core.Event.Logic;
using Scipts.Skills.Core.Model;
using Scipts.UnityAnimation.Component;

namespace Scipts.Skills.Combat.Event.Logic {
    public struct AnimationEventLogic : IEventLogic {
        public AnimationEventLogic(EntityManager manager, DefaultSkill parentSkill, BaseEvent @event) {
            var aei = (AnimationEventInfo) @event.eventInfo;
            ref var animation = ref manager.GetComponent<AnimationComponent>(parentSkill.dependencies.entity);
            animation.Play(aei.animationName, aei.method, aei.crossFade, aei.animationSpeed);
        }

        public void Enter() {
        }

        public void Update(float dt) {
        }

        public void LateUpdate(float dt) {
        }

        public void Exit() {
        }

        public bool IsActived {
            get => true;
        }

        public bool IsFinished {
            get => true;
        }
    }
}