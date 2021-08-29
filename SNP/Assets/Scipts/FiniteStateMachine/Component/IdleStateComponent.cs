using Scipts.EntityComponentSystem.Model;
using Scipts.FiniteStateMachine.Model;
using Scipts.UnityAnimation.Component;
using UnityEngine;

namespace Scipts.FiniteStateMachine.Component {
    [System.Serializable]
    public struct IdleStateComponent : IComponent {
        public const float TIME_REQUIREMENT_CAN_CHANGE_RUN_STATE = 0.1f;
        public float elapsed;
        public bool isRunning;
        [SerializeField] private int entity;

        public IdleStateComponent(int entity) {
            this.entity = entity;
            this.elapsed = 0.0f; 
            this.isRunning = false;
        }

        public void Enter(StateName @from, bool isContinue, EntityManager entityManager) {
            Debug.Log("idle enter");
            this.elapsed = 0.0f;
            this.isRunning = true;

            entityManager.GetComponent<AnimationComponent>(entity).PlayIdle();
        }

        public void Exit() {
            Debug.Log("idle exit");
            this.isRunning = false;
        }
    }
}