using EntityComponentSystem.Model;
using FiniteStateMachine.Component;
using FiniteStateMachine.Model;
using UserInput.Component;

namespace FiniteStateMachine.State {
    public struct RunState : IState {
        private EntityManager entityManager;
        private int entity;

        public RunState(EntityManager entityManager, int entity) {
            this.entityManager = entityManager;
            this.entity = entity;
        }

        public StateName StateName {
            get => StateName.RUN;
        }

        public void Enter(StateName @from, bool isContinue) {
            // todo: play animation run in here
        }

        public void Update(ref StateMachineComponent component, float dt) {
            ref var inputComponent = ref entityManager.GetComponent<InputComponent>(entity);

            if (inputComponent.isDash) {
                // change state to the dash state
                component.ListenChangeState(StateName.DASH, ChangeStateMethod.Backup);
                return;
            }

            if (inputComponent.isRunning) {
                // update request run
                return;
            }
            
            // go to the idle state
            component.ListenChangeState(StateName.IDLE, ChangeStateMethod.GoBack);
        }

        public void Exit() {
            
        }
    }
}