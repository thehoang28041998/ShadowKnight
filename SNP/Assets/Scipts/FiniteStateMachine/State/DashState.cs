using EntityComponentSystem.Model;
using FiniteStateMachine.Component;
using FiniteStateMachine.Model;
using Movement.Component;
using Movement.Request;
using UserInput.Component;

namespace FiniteStateMachine.State {
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
    }
}