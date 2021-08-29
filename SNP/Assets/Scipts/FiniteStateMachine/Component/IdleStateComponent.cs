using Scipts.EntityComponentSystem.Model;
using Scipts.FiniteStateMachine.Model;
using Scipts.UnityAnimation.Component;
using UnityEngine;

namespace Scipts.FiniteStateMachine.Component {
    public struct IdleStateComponent : IComponent {
        public const float TIME_REQUIREMENT_CAN_CHANGE_RUN_STATE = 0.1f;
        private readonly int entity;
        
        public float elapsed;
        public bool isRunning;

        public IdleStateComponent(int entity) {
            this.entity = entity;
            this.elapsed = 0.0f; 
            this.isRunning = false;
        }

        public void Enter(StateName @from, bool isContinue, EntityManager entityManager) {
            this.elapsed = 0.0f;
            this.isRunning = true;

            entityManager.GetComponent<AnimationComponent>(entity).PlayIdle();
        }

        public void Exit() {
            this.isRunning = false;
        }
    }
}