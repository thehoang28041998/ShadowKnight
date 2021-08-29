using Scipts.EntityComponentSystem.Model;
using Scipts.FiniteStateMachine.Model;
using Scipts.UnityAnimation.Component;
using UnityEngine;

namespace Scipts.FiniteStateMachine.Component {
    public struct RunStateComponent : IComponent {
        private readonly int entity;
        
        public float elaped;
        public bool isRunning;

        public RunStateComponent(int entity) {
            this.entity = entity;
            this.elaped = 0.0f;
            this.isRunning = false;
        }

        public void Enter(StateName @from, bool isContinue, EntityManager entityManager) {
            this.elaped = 0.0f;
            this.isRunning = true;

            entityManager.GetComponent<AnimationComponent>(entity).PlayRun();
        }

        public void Exit() {
            this.isRunning = false;
        }
    }
}