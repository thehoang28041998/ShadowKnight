using Leopotam.EcsLite;
using Scipts.EntityComponentSystem;
using Scipts.FiniteStateMachine.Model;
using Scipts.Movement.Component;
using Scipts.Movement.Request;
using Scipts.Skills.Component;
using Scipts.Skills.Model;
using Scipts.UnityAnimation.Component;
using Scipts.UserInput.Component;
using UnityEngine;

namespace Scipts.FiniteStateMachine.Component {
    public struct RunStateComponent : IComponent {
        private EntityManager entityManager;
        private EcsPool<InputComponent> pool1;
        private EcsPool<RequestComponent> pool2;
        private float elapsed;
        private bool isRunning;
        
        public RunStateComponent(EntityManager entityManager) {
            this.elapsed = 0.0f;
            this.isRunning = false;
            this.entityManager = entityManager;
            this.pool1 = entityManager.World.GetPool<InputComponent>();
            this.pool2 = entityManager.World.GetPool<RequestComponent>();
        }

        public void Enter(int entity, StateName previous, bool isContinue) {
            this.isRunning = true;
            this.elapsed = 0.0f;

            var newSkill = new SkillId(2, SkillCategory.Run, 1);
            var skill = entityManager.GetComponent<SkillComponent>(entity);
            skill.CastSkill(newSkill);
        }
        
        public void Run(ref StateMachineComponent stateMachine, int entity, float delta) {
            if (!isRunning) return;

            var input = pool1.Get(entity);

            if (input.isDash) {
                stateMachine.ListenChangeState(StateName.DASH, ChangeStateMethod.Backup);
                return;
            }

            if (input.isAttack) {
                stateMachine.ListenChangeState(StateName.ATTACK, ChangeStateMethod.Backup);
                return;
            }

            if (input.isRunning) {
                pool2.Get(entity).AddRequest(new RunRequest(input.direction.normalized));
                return;
            }
            
            stateMachine.ListenChangeState(StateName.IDLE, ChangeStateMethod.GoBack);
        }

        public void Exit() {
            isRunning = false;
        }
    }
}