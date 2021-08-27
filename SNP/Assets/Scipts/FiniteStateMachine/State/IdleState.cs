using EntityComponentSystem.Model;
using FiniteStateMachine.Component;
using FiniteStateMachine.Model;
using UserInput.Component;

namespace FiniteStateMachine.State {
    public struct IdleState : IState {
        private EntityManager entityManager;
        private int entity;

        public IdleState(EntityManager entityManager, int entity) {
            this.entityManager = entityManager;
            this.entity = entity;
        }

        public StateName StateName {
            get => StateName.IDLE;
        }

        public void Enter(StateName @from, bool isContinue) {
            // todo: play idle animation in here
        }

        public void Update(ref StateMachineComponent component, float dt) {
            ref var inputComponent = ref entityManager.GetComponent<InputComponent>(entity);

            if (inputComponent.isDash) {
                // change state to the dash state
                component.ListenChangeState(StateName.DASH, ChangeStateMethod.Backup);
                return;
            }

            if (inputComponent.isRunning) {
                // change state to the run state
                component.ListenChangeState(StateName.RUN, ChangeStateMethod.Backup);
                return;
            }
            
            // go to the idle state
        }

        public void Exit() {
            // nothing
        }
    }
}