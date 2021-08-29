using Scipts.EntityComponentSystem.Model;
using Scipts.FiniteStateMachine.Model;
using Scipts.FiniteStateMachine.State;
using Scipts.UnityAnimation.Component;
using Scipts.UserInput.Component;
using UnityEngine;

namespace Scipts.FiniteStateMachine.Component {
    [System.Serializable]
    public struct IdleStateComponent : IComponent, IState {
        public const float TIME_REQUIREMENT_CAN_CHANGE_RUN_STATE = 0.1f;
        public readonly EntityManager entityManager;
        private readonly int entity;
        
        public float elaped;
        public bool isRunning;

        public IdleStateComponent(EntityManager entityManager, int entity) {
            this.entityManager = entityManager;
            this.entity = entity;
            this.elaped = 0.0f;
            this.isRunning = false;
        }

        public StateName StateName {
            get => StateName.IDLE;
        }

        public void Enter(StateName @from, bool isContinue) {
            // todo: play idle animation in here
            this.elaped = 0.0f;
            entityManager.GetComponent<AnimationComponent>(entity).PlayIdle();
            this.isRunning = true;
        }

        public void Exit() {
            this.isRunning = false;
        }
    }
}