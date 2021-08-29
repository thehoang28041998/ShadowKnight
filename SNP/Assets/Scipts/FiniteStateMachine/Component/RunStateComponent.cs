using Scipts.EntityComponentSystem.Model;
using Scipts.FiniteStateMachine.Model;
using Scipts.UnityAnimation.Component;
using UnityEngine;

namespace Scipts.FiniteStateMachine.Component {
    [System.Serializable]
    public struct RunStateComponent : IComponent {
        public float elaped;
        public bool isRunning;
        [SerializeField] private int entity;

        public RunStateComponent(int entity) {
            this.entity = entity;
            this.elaped = 0.0f;
            this.isRunning = false;
        }

        public void Enter(StateName @from, bool isContinue, EntityManager entityManager) {
            Debug.Log("run enter");
            this.elaped = 0.0f;
            this.isRunning = true;

            entityManager.GetComponent<AnimationComponent>(entity).PlayRun();
        }

        public void Exit() {
            Debug.Log("run exit");
            this.isRunning = false;
        }
    }
}