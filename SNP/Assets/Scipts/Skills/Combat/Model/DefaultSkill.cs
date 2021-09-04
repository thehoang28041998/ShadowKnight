using System.Collections.Generic;
using Scipts.EntityComponentSystem;
using Scipts.EntityComponentSystem.Component;
using Scipts.Skills.Core.Config;
using Scipts.Skills.Core.Event;
using Scipts.Skills.Core.Event.Logic;
using Scipts.Skills.Core.Model;
using Scipts.Skills.Model;
using Scipts.UnityAnimation.Component;
using Scipts.UnityAnimation.Model;
using UnityEngine;

namespace Scipts.Skills.Combat.Model {
    public partial class DefaultSkill : BaseSkill {
        public readonly Dependencies dependencies;
        private readonly EntityManager manager;
        private readonly List<int> projectiles = new List<int>();

        public DefaultSkill(EntityManager manager, Dependencies dependencies) : base(dependencies.skillConfig) {
            this.manager = manager;
            this.dependencies = dependencies;
        }

        public override void OnCast(SkillId skillId) {
            base.OnCast(skillId);
            PlayAnimationConfig();
        }

        protected override void OnUpdate(float dt) {
        }

        public override void OnFinish() {
            Debug.Log($"{dependencies.skillConfig.name} is finish");
        }

        public override void Destroy() {
            base.Destroy();
            foreach (var entity in projectiles) {
                manager.GetComponent<CleanupComponent>(entity).ForceCleanup();
            }
        }

        protected override void LaunchProjectile(BaseEvent be) {
            int newEntity = manager.NewEntity();
            projectiles.Add(newEntity);
            
            CompletedInitProjectileInit(newEntity, be);
        }

        protected virtual void CompletedInitProjectileInit(int projectile, BaseEvent be) {
        }

        protected override IEventLogic GenerationEventLogic(BaseEvent be) {
            return EventFactory.GenerationEventLogic(this, be);
        }
        
        private void PlayAnimationConfig() {
            var config = dependencies.skillConfig;
            string animationName = config.animationName;
            float animationSpeed = config.animationSpeed;

            if (string.IsNullOrEmpty(animationName)) return;

            ref var animation = ref manager.GetComponent<AnimationComponent>(dependencies.entity);
            if (animationName.Equals(AnimationComponent.RUN)) {
                animation.PlayRun();
                return;
            }
            
            animation.StopIfDontLoop(animationName);
            animation.Play(animationName, AnimationPlayMethod.Play, 0.0f, animationSpeed);
        }
    }

    public partial class DefaultSkill {
        public class Dependencies {
            public readonly SkillConfig skillConfig;
            public readonly int entity;

            public Dependencies(SkillConfig skillConfig, int entity) {
                this.entity = entity;
                this.skillConfig = skillConfig;
            }
        }
    }
}