using Scipts.EntityComponentSystem.Model;
using Scipts.FiniteStateMachine.Component;
using Scipts.FiniteStateMachine.Model;
using Scipts.Movement.Component;
using Scipts.Movement.Request;
using Scipts.UnityAnimation.Component;

namespace Scipts.FiniteStateMachine.State {
    public struct DashState : IState {
        private EntityManager entityManager;
        private int entity;

        public DashState(EntityManager entityManager, int entity) {
            this.entityManager = entityManager;
            this.entity = entity;
        }

        public StateName StateName {
            get => StateName.DASH;
        }

        public void Enter(StateName @from, bool isContinue) {
            // todo: play animation dash in here
            entityManager.GetComponent<AnimationComponent>(entity).PlayDash();

            // todo: start dash request
            var velocity = entityManager.GetComponent<VelocityComponent>(entity).saveVelocity;
            entityManager.GetComponent<RequestComponent>(entity).AddRequest(new DashRequest(10, 0.3f, velocity.normalized));
           
            // todo: register listen dash behaviour
        }

        public void Update(ref StateMachineComponent component, float dt) {
            // todo: if request complete -> go to back previous the state
            DashComponent dashComponent = entityManager.GetComponent<DashComponent>(entity);
            if (dashComponent.IsFinish) {
                component.ListenChangeState(StateName.IDLE, ChangeStateMethod.GoBack);
            }
        }

        public void Exit() {
            // todo: remove listen dash behaviour
        }

        public void GUI() {
            
        }
    }
}