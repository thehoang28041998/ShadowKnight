using Scipts.EntityComponentSystem.Model;
using Scipts.FiniteStateMachine.Model;
using Scipts.Movement.Component;
using Scipts.Movement.Request;
using Scipts.UnityAnimation.Component;

namespace Scipts.FiniteStateMachine.Component {
    public struct DashStateComponent : IComponent {
        private int entity;
        public bool isRunning;
        
        public DashStateComponent(int entity) {
            this.entity = entity;
            this.isRunning = false;
        }

        public void Enter(StateName @from, bool isContinue, EntityManager entityManager) { 
            // todo: play animation dash in here
            entityManager.GetComponent<AnimationComponent>(entity).PlayDash();

            // todo: start dash request
            var velocity = entityManager.GetComponent<VelocityComponent>(entity).saveVelocity;
            entityManager.GetComponent<RequestComponent>(entity).AddRequest(new DashRequest(10, 0.3f, velocity.normalized));
           
            // todo: register listen dash behaviour
            this.isRunning = true;
        }

        public void Exit() {
            this.isRunning = false;
        }
    }
}